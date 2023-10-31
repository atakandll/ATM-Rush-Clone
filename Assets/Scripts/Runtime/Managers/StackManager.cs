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

        #region Public Variables

        public StackJumperCommand StackJumperCommand { get; private set; }

        public StackTypeUpdaterCommand StackTypeUpdaterCommand { get; private set; }

        public ItemAdderOnStackCommand AdderOnStackCommand { get; private set; }

        public bool LastCheck { get; set; }

        #endregion

        #region Seralized Veriables

        [SerializeField] private GameObject money;

        #endregion

        #region Private Variables

        private StackData _data;
        private List<GameObject> _collectableStack = new List<GameObject>();

        private StackMoverCommand _stackMoverCommand;
        private ItemRemoverOnStackCommand _itemRemoverOnStackCommand;
        private StackAnimatorCommand _stackAnimatorCommand;
        private StackInteractionWithConveyorCommand _stackInteractionWithConveyorCommand;
        private StackInitializerCommand _stackInitializerCommand;

        private readonly string _stackDataPath = "Data/CD_Stack";

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
            AdderOnStackCommand = new ItemAdderOnStackCommand(this, ref _collectableStack, ref _data);
            _itemRemoverOnStackCommand = new ItemRemoverOnStackCommand(this, ref _collectableStack);
            _stackAnimatorCommand = new StackAnimatorCommand(this, _data, ref _collectableStack);
            StackJumperCommand = new StackJumperCommand(_data, ref _collectableStack);
            _stackInteractionWithConveyorCommand = new StackInteractionWithConveyorCommand(this, ref _collectableStack);
            StackTypeUpdaterCommand = new StackTypeUpdaterCommand(ref _collectableStack);
            _stackInitializerCommand = new StackInitializerCommand(this, ref money);
        }

        private StackData GetStackData()
        {
            return Resources.Load<CD_Stack>(_stackDataPath).Data;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            StackSignals.Instance.onInteractionCollectable += OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle += _itemRemoverOnStackCommand.Execute;
            StackSignals.Instance.onInteractionATM += OnInteractionWithATM;
            StackSignals.Instance.onInteractionConveyor +=
                _stackInteractionWithConveyorCommand.Execute;
            StackSignals.Instance.onStackFollowPlayer += OnStackMove;
            StackSignals.Instance.onUpdateType += StackTypeUpdaterCommand.Execute;
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

        private void OnInteractionWithATM(GameObject collectableGameObject)
        {
            ScoreSignals.Instance.onSetAtmScore?.Invoke((int)collectableGameObject.GetComponent<CollectableManager>()
                .GetCurrentValue() + 1);
            if (LastCheck == false)
            {
                _itemRemoverOnStackCommand.Execute(collectableGameObject);
            }
            else
            {
                collectableGameObject.SetActive(false);
            }
        }

        private void OnInteractionWithCollectable(GameObject collectableGameObject)
        {
            DOTween.Complete(StackJumperCommand);
            AdderOnStackCommand.Execute(collectableGameObject);
            StartCoroutine(_stackAnimatorCommand.Execute());
            StackTypeUpdaterCommand.Execute();
        }

        private void OnPlay()
        {
            _stackInitializerCommand.Execute();
        }


        private void UnSubscribeEvents()
        {
            StackSignals.Instance.onInteractionCollectable -= OnInteractionWithCollectable;
            StackSignals.Instance.onInteractionObstacle -= _itemRemoverOnStackCommand.Execute;
            StackSignals.Instance.onInteractionATM -= OnInteractionWithATM;
            StackSignals.Instance.onInteractionConveyor -=
                _stackInteractionWithConveyorCommand.Execute;
            StackSignals.Instance.onStackFollowPlayer -= OnStackMove;
            StackSignals.Instance.onUpdateType -= StackTypeUpdaterCommand.Execute;
            CoreGameSignals.Instance.onPlay -= OnPlay;
            CoreGameSignals.Instance.onReset -= OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void OnReset()
        {
            LastCheck = false;
            _collectableStack.Clear();
            _collectableStack.TrimExcess();
        }
    }
}