using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Commands.Stack;
using Runtime.Data.UnityObject;
using Runtime.Data.ValueObject;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Runtime.Managers
{
    public class StackManager : MonoBehaviour
    {
        #region Self Variables

        #region  Public Variables

        public StackJumperCommand StackJumperCommand => _stackJumperCommand;
        public StackTypeUpdaterCommand StackTypeUpdaterCommand => _stackTypeUpdaterCommand;

        #endregion
        
        #region Serialized Variables
        
        [SerializeField] private GameObject levelHolder;
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
            _stackMoverCommand = new StackMoverCommand(this, ref _data);
            _itemAdderOnStackCommand = new ItemAdderOnStackCommand(this,ref _collectableStack, ref _data);
            _itemRemoverOnStackCommand = new ItemRemoverOnStackCommand(this, ref _collectableStack);
            _stackAnimatorCommand = new StackAnimatorCommand(this, ref _data, ref _collectableStack);
            _stackJumperCommand = new StackJumperCommand(ref _data, ref _collectableStack);
            _stackInteractionWithConveyorCommand = new StackInteractionWithConveyorCommand(this, ref _collectableStack);
            _stackTypeUpdaterCommand = new StackTypeUpdaterCommand(ref _collectableStack);
            _stackInitializerCommand = new StackInitializerCommand(this, ref _collectableStack);
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
            CoreGameSignals.Instance.onPlay += OnPlay;
            CoreGameSignals.Instance.onReset += OnReset;
        }
        
        private void OnPlay()
        {
            _stackInitializerCommand.Execute();
        }

        private void UnSubscribeEvents()
        {
            StackSignals.Instance.onInteractionCollectable -= OnInteractionCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoverOnStackCommand.Execute;
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
            
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(0).gameObject);
            }
            _collectableStack.Clear(); // not just clear we must do trimExcess
            _collectableStack.TrimExcess(); // clear 
        }
    }
}