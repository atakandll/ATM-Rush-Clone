using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Commands.Stack;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class StackManager : MonoBehaviour
    {
        #region Self Variables

        #region  Public Variables

        public StackJumperCommand StackJumperCommand => _stackJumperCommand; // just get operation
        public StackTypeUpdaterCommand StackTypeUpdaterCommand => _stackTypeUpdaterCommand;
        public ItemAdderOnStackCommand ItemAdderOnStackCommand => _itemAdderOnStackCommand;

        public bool LastCheck
        {
            get => _lastCheck;
            set => _lastCheck = value;
        }

        #endregion
        
        #region Serialized Variables
        
        [SerializeField] private GameObject money;
        #endregion


        #region Private Variables

        [ShowInInspector] private StackData _data;
        private List<GameObject> _collectableStack = new List<GameObject>();
        private bool _lastCheck;
        
        private StackMoverCommand _stackMoverCommand; // stack lerp move
        private ItemAdderOnStackCommand _itemAdderOnStackCommand; // add list new item
        private ItemRemoverOnStackCommand _itemRemoverOnStackCommand; // remove list old item
        private StackAnimatorCommand _stackAnimatorCommand; // scale up adding item and remove item
        private StackJumperCommand _stackJumperCommand; // collison with obstacles and jump forces
        private StackInteractionWithConveyorCommand _stackInteractionWithConveyorCommand; // collison with mini game
        private StackTypeUpdaterCommand _stackTypeUpdaterCommand; // update stack according to type
        private StackInitializerCommand _stackInitializerCommand;

        private readonly string stackDataPath = "Data/CD_Stack";
        
        #endregion

        #endregion

        private void Awake()
        {
            _data = GetStackData();
            Init();
        }

        private void Init()
        {
            _stackMoverCommand = new StackMoverCommand(ref _data);
            _itemAdderOnStackCommand = new ItemAdderOnStackCommand(this,ref _collectableStack, ref _data);
            _itemRemoverOnStackCommand = new ItemRemoverOnStackCommand(this, ref _collectableStack);
            _stackAnimatorCommand = new StackAnimatorCommand(this, ref _data, ref _collectableStack);
            _stackJumperCommand = new StackJumperCommand(ref _data, ref _collectableStack);
            _stackInteractionWithConveyorCommand = new StackInteractionWithConveyorCommand(this, ref _collectableStack);
            _stackTypeUpdaterCommand = new StackTypeUpdaterCommand(ref _collectableStack);
            _stackInitializerCommand = new StackInitializerCommand(this, ref money);
        }

        private StackData GetStackData()
        {
            return Resources.Load<CD_Stack>(stackDataPath).Data;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            StackSignals.Instance.onInteractionCollectable += OnInteractionCollectable;
            StackSignals.Instance.onInteractionObstacle += _itemRemoverOnStackCommand.Execute;
            StackSignals.Instance.onInteractionATM += OnInteractionAtm;
            StackSignals.Instance.onInteractionConveyor += _stackInteractionWithConveyorCommand.Execute;
            StackSignals.Instance.onStackFollowPlayer += OnStackMove;
            StackSignals.Instance.onUpdateType += _stackTypeUpdaterCommand.Execute;
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
        }

        private void OnStackMove(Vector2 direction)
        {
            transform.position = new Vector3(0, gameObject.transform.position.y, direction.y + 2f);
            
            if (gameObject.transform.childCount > 0)
            {
                _stackMoverCommand.Execute(direction.x, _collectableStack);
            }
        }

        private void OnInteractionAtm(GameObject collectableGameobject)
        {
            ScoreSignals.Instance.onSetAtmScore?.Invoke((int)collectableGameobject.GetComponent<CollectableManager>()
                .GetCurrentValue() + 1);
             
            if (_lastCheck == false)
            {
                _itemRemoverOnStackCommand.Execute(collectableGameobject);
            }
            else // last atm on game check
            {
                collectableGameobject.SetActive(false);
            }
        }

        private void OnPlay()
        {
            _stackInitializerCommand.Execute();
        }

        private void UnSubscribeEvents()
        {
            StackSignals.Instance.onInteractionCollectable -= OnInteractionCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoverOnStackCommand.Execute;
            StackSignals.Instance.onInteractionATM -= OnInteractionAtm;
            StackSignals.Instance.onInteractionConveyor -= _stackInteractionWithConveyorCommand.Execute;
            StackSignals.Instance.onStackFollowPlayer = OnStackMove;
            StackSignals.Instance.onUpdateType -= _stackTypeUpdaterCommand.Execute;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnInteractionCollectable(GameObject collectableGameobject)
        {
            DOTween.Complete(_stackJumperCommand);
            _itemAdderOnStackCommand.Execute(collectableGameobject);
            StartCoroutine(_stackAnimatorCommand.Execute());
            _stackTypeUpdaterCommand.Execute();
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
        private void OnReset()
        {
            _lastCheck = false;
            _collectableStack.Clear(); // not just clear we must do trimExcess
            _collectableStack.TrimExcess(); // clear 
        }
    }
}