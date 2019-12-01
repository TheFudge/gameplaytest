using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        private static bool isInstanced = false;
        private static GameManager instance;

        public static GameManager Instance
        {
            get
            {
                if (!isInstanced)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                    isInstanced = true;
                }

                return instance;
            }
        }

        public FollowCharacter CameraFollow;
        public Character Character;
        public CharacterController CharacterController;

        void Awake()
        {
            instance = this;
            isInstanced = true;
        }
    }

}