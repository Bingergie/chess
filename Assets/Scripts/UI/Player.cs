using System;
using Unity.VisualScripting;
using UnityEngine;

namespace UI {
    [RequireComponent(typeof(Camera))]
    public class Player : MonoBehaviour {
        [HideInInspector] public bool isWhite;
        private PlayerState state;
        private Coord selectedSquare;
        private new Camera camera;

        private void Start() {
            GameManager.OnGameStart += OnGameStart;
        }

        private void OnGameStart(object sender, bool isWhiteSide) {
            isWhite = isWhiteSide;
            state = isWhiteSide ? PlayerState.Idle : PlayerState.Waiting;
        }

        private void Awake() {
            camera = GetComponent<Camera>();
        }

        public bool GetMouseTarget(out GameObject target, LayerMask layerMask = default) {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            bool didHit = Physics.Raycast(ray, out var hit, Mathf.Infinity, layerMask);
            target = didHit ? hit.transform.gameObject : null;
            return didHit;
        }

        private void Update() {
            switch (state) {
                case PlayerState.Idle:
                    IdleUpdate();
                    break;
                case PlayerState.Dragging:
                    DraggingUpdate();
                    break;
                default:
                    break;
            }
        }

        private void IdleUpdate() {
            if (Input.GetMouseButtonDown(0)) {
                if (GetMouseTarget(out var hit, LayerMask.GetMask("Board")) &&
                    hit.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled) {
                    Debug.Log(hit.transform.gameObject.name); // todo: delete this

                    selectedSquare = BoardUtil.CoordFromName(hit.transform.gameObject.name);
                    state = PlayerState.Dragging;
                }
            }
        }

        private void DraggingUpdate() {
            /*if (Input.GetMouseButtonDown(0)) {
                if (GetMouseTarget(out var hit, LayerMask.GetMask("Board")) &&
                    BoardUtil.CoordFromName(hit.transform.gameObject.name) == selectedSquare) {
                    selectedSquare = default;
                    state = PlayerState.Idle;
                    return;
                }
            }*/
            if (Input.GetMouseButton(0)) {
                // OnMouseDrag?.Invoke(this, Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0)) {
                if (GetMouseTarget(out var hit, LayerMask.GetMask("Board"))) {
                    if (BoardUtil.CoordFromName(hit.transform.gameObject.name) == selectedSquare) {
                        return;
                    }

                    // Debug.Log(hit.transform.gameObject.name); // todo: delete this
                    GameManager.Instance.TryMakeMove(new Move(selectedSquare,
                        BoardUtil.CoordFromName(hit.transform.gameObject.name)));
                }

                state = PlayerState.Idle;
                selectedSquare = default;
            }
        }
    }

    public enum PlayerState {
        Idle,
        Dragging,
        Waiting
    }
}