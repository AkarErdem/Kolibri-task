using System.Collections.Generic;
using UnityEngine;

namespace GameCode.Base
{
    public abstract class MVCCollection<TKey, TModel, TView, TController> where TView : MonoBehaviour
    {
        private readonly Dictionary<TKey, TModel> _models;
        private readonly Dictionary<TKey, TView> _views;
        private readonly Dictionary<TKey, TController> _controllers;

        protected MVCCollection()
        {
            _views = new Dictionary<TKey, TView>();
            _models = new Dictionary<TKey, TModel>();
            _controllers = new Dictionary<TKey, TController>();
        }

        public void Register(TKey key, TModel model, TView view, TController controller)
        {
            _views.Add(key, view);
            _models.Add(key, model);
            _controllers.Add(key, controller);
        }

        public int GetCount()
        {
            return _models.Count;
        }

        public TModel GetModel(TKey key)
        {
            return _models[key];
        }
        public TView GetView(TKey key)
        {
            return _views[key];
        }
        public TController GetController(TKey key)
        {
            return _controllers[key];
        }
        
        public Dictionary<TKey, TModel> GetModels()
        {
            return new Dictionary<TKey, TModel>(_models);
        }
        public Dictionary<TKey, TView> GetViews()
        {
            return new Dictionary<TKey, TView>(_views);
        }
        public Dictionary<TKey, TController> GetControllers()
        {
            return new Dictionary<TKey, TController>(_controllers);
        }
    }
}
