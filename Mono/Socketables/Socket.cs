namespace PofyTools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using EnumExtensions;

    public class Socket : MonoBehaviour, ITransformable, IInitializable
    {
        public enum Action:int
        {
            // default
            None = 0,
            // equip item to owner
            Equip = 1,
            // unequip item from owner
            Unequip = 2,
            // unequip all items
            Empty = 3,
            //TODO: Add socket to owner
            Add = 4,
            //TODO Remove socket from owner
            Remove = 5,
        }

        public string id;
        public ISocketed owner;
        public int itemLimit = 1;
        protected List<ISocketable> _items = new List<ISocketable>();

        public List<ISocketable> items
        {
            get
            {
                if (this._items == null)
                    this._items = new List<ISocketable>(this.itemLimit);

                return this._items;
            }
        }

        public bool hasAnyItem
        {
            get
            { 
                if (this._items == null)
                    this._items = new List<ISocketable>(this.itemLimit);

                return this._items.Count > 0;
            }
        }

        public int itemCount
        {
            get
            {
                if (this._items == null)
                    this._items = new List<ISocketable>(this.itemLimit);
                return this._items.Count;
            }
        }

        public bool Empty(SocketActionRequest.ApprovedBy approvedBy = SocketActionRequest.ApprovedBy.None)
        {
            for (int i = 0, max_itemsCount = this._items.Count; i < max_itemsCount; i++)
            {
                var item = this._items[i];
                SocketActionRequest.TryUnequipItemFromOwner(this.owner, item, this.id, approvedBy);
            }

            return this._items.Count > 0;
        }

        public void AddItem(SocketActionRequest request)
        {
            this._items.Add(request.item);
            request.item.Equip(request);
            //TODO: Owner callback
        }

        public bool RemoveItem(SocketActionRequest request)
        {
            if (this._items.Remove(request.item))
            {
                request.item.Unequip(request);
                //TODO: Owner callback
                return true;
            }

            return false;
        }

        #region ITransformable implementation

        protected Transform _selfTransform;

        public Transform selfTransform
        {
            get
            {
                return this._selfTransform;
            }
        }

        #endregion

        #region IInitializable implementation

        public bool Initialize()
        {
            if (!this._isInitialized)
            {
                this.owner = GetComponentInParent<ISocketed>();

                if (this.owner != null)
                {
                    this.owner.AddSocket(this);
                    this._selfTransform = this.transform;


                    GetComponentsInChildren<ISocketable>(this._items);
                    SocketActionRequest request = new SocketActionRequest(socket: this);
                    foreach (var item in this._items)
                    {
                        item.Initialize();
                        item.Equip(request);
                    }
                    return true;
                }
            }
            return false;
        }

        protected bool _isInitialized = false;

        public bool isInitialized
        {
            get
            {
                return this._isInitialized;
            }
        }

        #endregion

        void Awake()
        {
            Initialize();
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(this.transform.position, 0.1f);
        }
        #endif

    }

    public struct SocketActionRequest
    {
        public const string TAG = "<color=green><b>SocketActionRequest:</b></color> ";

        public enum ApprovedBy:int
        {
            None = 0,
            SocketOwner = 1 << 0,
            Item = 1 << 1
        }

        #region Variables

        private SocketActionRequest.ApprovedBy _approvedBy;

        public SocketActionRequest.ApprovedBy approvedBy
        {
            get
            {
                return _approvedBy;
            }
            private set
            {
                this._approvedBy = value;
            }
        }

        private Socket.Action _action;

        public Socket.Action action
        {
            get
            {
                return _action;
            }
            private set
            {
                this._action = value;
            }
        }

        private ISocketable _item;

        public ISocketable item
        {
            get
            {
                return _item;
            }
            private set
            {
                this._item = value;
            }
        }

        private ISocketed _owner;

        public ISocketed owner
        {
            get
            {
                return _owner;
            }
            private set
            {
                this._owner = value;
            }
        }

        private string _id;

        public string id
        {
            get
            {
                return _id;
            }
            private set
            {
                this._id = value;
            }
        }

        private Socket _socket;

        public Socket socket
        {
            get
            {
                if (this._socket == null)
                {
                    if ((int)this._action <= 2)
                        this._socket = this._owner.GetSocket(this.id);
                }

                return this._socket;
            }
        }

        #endregion

        #region Instance Methods

        public bool isAprovedByAll
        {
            get
            {
                return this.approvedBy.Has(ApprovedBy.SocketOwner) && this.approvedBy.Has(ApprovedBy.SocketOwner);
            }
        }

        public void ApproveByOwner(ISocketed owner)
        {
            if (owner == this._owner)
                this.approvedBy = this.approvedBy.Add(ApprovedBy.SocketOwner);
        }

        public void ApproveByItem(ISocketable item)
        {
            if (item == this._item)
                this.approvedBy = this.approvedBy.Add(ApprovedBy.Item);
        }

        public void RevokeApproval()
        {
            this.approvedBy = ApprovedBy.None;
        }

        public void ForceApproval()
        {
            this._approvedBy = this._approvedBy.Add(ApprovedBy.SocketOwner);
            this._approvedBy = SocketActionRequest.ApprovedByAll;
        }

        #endregion

        #region Constructor

        public SocketActionRequest(Socket.Action action = Socket.Action.None, ISocketed owner = null, ISocketable item = null, string id = "", ApprovedBy approvedBy = ApprovedBy.None, Socket socket = null)
        {
            this._action = action;
            this._owner = owner;
            this._item = item;
            this._id = id;
            this._approvedBy = approvedBy;
            this._socket = socket;
        }

        //        public SocketActionRequest(Socket socket) : this(Socket.Action.None,socket.owner,null,socket.id,ApprovedBy.None)

        #endregion

        #region Object

        public override string ToString()
        {
            return string.Format("[SocketActionRequest: approvedBy={0}, action={1}, item={2}, owner={3}, id={4}, socket={5}, isAprovedByAll={6}]", approvedBy, action, item, owner, id, socket, isAprovedByAll);
        }

        #endregion

        #region API Methods

        public static SocketActionRequest TryEquipItemToOwner(ISocketed owner, ISocketable item, string id, ApprovedBy approvedBy = ApprovedBy.None)
        {
            SocketActionRequest request = new SocketActionRequest(action: Socket.Action.Equip, owner: owner, item: item, id: id, approvedBy: approvedBy);

            return ResolveRequest(request);
        }

        public static SocketActionRequest TryUnequipItemFromOwner(ISocketed owner, ISocketable item, string id = "", ApprovedBy approvedBy = ApprovedBy.None)
        {
            SocketActionRequest request = new SocketActionRequest(action: Socket.Action.Unequip, owner: owner, item: item, id: id, approvedBy: approvedBy);
            return ResolveRequest(request);
        }

        public static SocketActionRequest TryEmptySocket(Socket socket, ApprovedBy approvedBy = ApprovedBy.None)
        {
            SocketActionRequest request = new SocketActionRequest(action: Socket.Action.Empty, owner: socket.owner, item: null, id: socket.id, approvedBy: approvedBy);
            return ResolveRequest(request);
        }

        /// <summary>
        /// Gets the approval from socket owner first and socketable item second. Returns request with resolved approvedBy field.
        /// </summary>
        /// <returns>Resolved request.</returns>
        /// <param name="request">Request.</param>
        public static SocketActionRequest GetApproval(SocketActionRequest request)
        {
            if (!request.isAprovedByAll)
            {
                if (!request.approvedBy.Has(ApprovedBy.SocketOwner))
                    request = request.owner.ResolveRequest(request);
                if (!request.approvedBy.Has(ApprovedBy.Item))
                    request = request.item.ResolveRequest(request);
            }

            return request;
        }

        /// <summary>
        /// Resolves the request. Resulting in socketing an item to it's owner's socket or ignoring the action.
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="request">Request.</param>
        public static SocketActionRequest ResolveRequest(SocketActionRequest request)
        {
            Socket socket = null;
            if (request.action == Socket.Action.None)
            {
                return request;
            }

            if (request.action == Socket.Action.Empty)
            {
                request.socket.Empty(request.approvedBy);
                return request;
            }


            if (request.action == Socket.Action.Add || request.action == Socket.Action.Remove)
            {
                if (!request.approvedBy.Has(SocketActionRequest.ApprovedBy.SocketOwner))
                    request = request.owner.ResolveRequest(request);
                
                if (request.approvedBy.Has(SocketActionRequest.ApprovedBy.SocketOwner))
                {
                    if (request.action == Socket.Action.Add)
                        request.owner.AddSocket(request.socket);
                    else
                        request.owner.RemoveSocket(request.socket);
                }

                return request;
            }



            request = SocketActionRequest.GetApproval(request);

            if (request.isAprovedByAll)
            {
                if (request.action == Socket.Action.Equip)
                {    
                    socket = request.socket;
                    socket.AddItem(request);
                    return request;
                }

                if (request.action == Socket.Action.Unequip)
                {
                    socket = request.item.socket;
                    socket.RemoveItem(request);
                    return request;
                }
            }

            Debug.LogWarning(TAG + request.ToString() + "was rejected!");

            return request;
        }

        public static ApprovedBy ApprovedByAll
        {
            get
            {
                return ApprovedBy.SocketOwner | ApprovedBy.Item;
            }
        }

        #endregion
    }

    public interface ISocketed : ITransformable, IInitializable, ISocketActionRequestResolver // Character
    {
        bool hasAnyItem{ get; }

        bool AddSocket(Socket socket);

        bool RemoveSocket(Socket socket);

        Socket GetSocket(string id);

        List<string> GetIds();

        List<ISocketable> GetItems();

        bool UnequipAll(SocketActionRequest.ApprovedBy approvedBy = SocketActionRequest.ApprovedBy.None);

        bool TryGetItemsOfType<T>(out List<T> list);
    }

    public interface ISocketable : ITransformable,IInitializable, ISocketActionRequestResolver// Item
    {
        Socket socket
        {
            get;
            set;
        }

        bool isEquipped
        {
            get;
        }

        void Equip(SocketActionRequest request);

        void Unequip(SocketActionRequest request);

    }

    public interface ISocketActionRequestResolver
    {
        SocketActionRequest ResolveRequest(SocketActionRequest request);
    }
        
    public delegate void SocketActionRequestDelegate(SocketActionRequest request);
    public delegate void SocketableDelegate(ISocketable socketable);
    public delegate void SocketedDelegate(ISocketed socketed);
}