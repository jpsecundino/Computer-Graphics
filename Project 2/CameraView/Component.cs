using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World_3D.CameraView
{
    public abstract class Component
    {
        public GameObject parent;

        public abstract void OnStart();
        public abstract void OnUpdate();
        public abstract void OnDestroy();

    }
}
