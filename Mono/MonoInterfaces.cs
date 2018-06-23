namespace PofyTools
{
    using UnityEngine;

    public interface ICollidable
    {
        Rigidbody SelfRigidbody
        {
            get;
        }

        Collider SelfCollider
        {
            get;
        }
    }

    public interface ICollidable2D
    {
        Rigidbody2D SelfRigidbody2D
        {
            get;
        }

        Collider2D SelfCollider2D
        {
            get;
        }
    }

    public interface ITransformable
    {
        Transform SelfTransform
        {
            get;
        }
    }

    public interface IRenderable
    {
        MeshRenderer SelfRenderer
        {
            get;
        }
    }

    public interface IRenderable2D
    {
        SpriteRenderer SelfRenderer2D
        {
            get;
        }
    }

    public interface IAnimated
    {
        Animator  SelfAnimator
        {
            get;
        }
    }

    public interface IStateMachine
    {
        void AddState(UpdateDelegate state);

        void RemoveState(UpdateDelegate state);

        void SetToState(UpdateDelegate state);

        void RemoveAllStates();

        void StackState(UpdateDelegate state);

        UpdateDelegate CurrentState
        {
            get;
        }
    }

    //	public interface IInteractable:IIdentifiable
    //	{
    //		InteractionDescription interactionDescription {
    //			get;
    //		}
    //
    //		void Interact (IInteractable other);
    //
    //		void SelfInteract ();
    //
    //		void SelfInteractCustom (InteractionDescription interactionDescription);
    //	}

    public interface ICollisionListener
    {
        void CollisionDetected(CollisionDetector detector, Collision collision);

        void CollisionStay(CollisionDetector detector, Collision collision);

        void CollisionEnded(CollisionDetector detector, Collision collision);
    }

    public interface ICollisionListener2D
    {
        void CollisionDetected(CollisionDetector2D detector, Collision2D collision);

        void CollisionStay(CollisionDetector2D detector, Collision2D collision);

        void CollisionEnded(CollisionDetector2D detector, Collision2D collision);
    }

    public interface ITriggerListener
    {
        void TriggerDetected(TriggerDetector detector, Collider other);

        void TriggerStay(TriggerDetector detector, Collider other);

        void TriggerEnded(TriggerDetector detector, Collider other);
    }

    public interface ITriggerListener2D
    {
        void TriggerDetected(TriggerDetector2D detector, Collider2D other);

        void TriggerStay(TriggerDetector2D detector, Collider2D other);

        void TriggerEnded(TriggerDetector2D detector, Collider2D other);
    }

    [System.Serializable]
    public class InteractionDescription
    {
        public string sound;
        public string effect;
    }

    public interface IInitializable
    {
        bool IsInitialized
        {
            get;
        }

        bool Initialize();
    }

    public interface ISubscribable
    {
        bool Subscribe();

        bool Unsubscribe();

        bool IsSubscribed
        {
            get;
        }
    }

    public interface IToggleable
    {
        void Toggle();

        void Toggle (bool on);

        void Close();

        void Open();

        bool IsOpen{ get; }
    }

    public interface IActivatable
    {
        void Activate();

        bool Update(float deltaTime);

        void Deactivate();
    }

    public interface IComposable<T>
    {
        void Compose(T description);

        void Decompose();
    }

    public interface IComposable
    {
        void Compose();

        void Decompose();
    }

    public interface IBackButtonListener
    {
        bool OnBackButton();
    }

}
