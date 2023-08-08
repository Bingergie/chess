using System;
using Riptide;
using Shared;
using UnityEngine;

namespace Client {
    public class PlayerManager : Singleton<PlayerManager> {
        public static event EventHandler<Coord> OnSquareSelected;
        public static event EventHandler<Coord> OnSquareDeselected;
        public static event EventHandler<Ray> OnPieceDragged;

        private Camera _mainCamera;
        private Coord _selectedSquare;

        private void Start() {
            _mainCamera = Camera.main;
            _selectedSquare = Coord.None;
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                if (GetTargetSquare(out var targetCoord)) {
                    _selectedSquare = targetCoord;
                    OnSquareSelected?.Invoke(this, targetCoord);
                }
            }

            if (Input.GetMouseButton(0)) {
                var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                if (_selectedSquare != null) {
                    OnPieceDragged?.Invoke(this, ray);
                }
            }

            if (Input.GetMouseButtonUp(0)) {
                if (_selectedSquare != Coord.None) {
                    OnSquareDeselected?.Invoke(this, _selectedSquare);
                    if (GetTargetSquare(out var targetCoord)) {
                        TryMakeMove(_selectedSquare, targetCoord);
                    }
                    _selectedSquare = Coord.None;
                }
            }
        }
        
        private bool GetTargetSquare(out Coord targetCoord) {
            string target;
            var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            /*if (Physics.Raycast(ray, out var hit, LayerMask.GetMask("Piece"))) {
                target = hit.transform.parent.name;
            }
            else */if (Physics.Raycast(ray, out var hitBoard, LayerMask.GetMask("Board"))) {
                target = hitBoard.transform.name;
            }
            else {
                targetCoord = Coord.None;
                return false;
            }

            Debug.Log(target);
            targetCoord = BoardUtil.CoordFromName(target);
            return true;
        }

        private void TryMakeMove(Coord from, Coord to) {
            var move = new Move(from, to);
            GameManager.Instance.TryMakeMove(move);
        }
    }
}