using System;
using UnityEngine;
using LittleSword.InputSystem;
using LittleSword.Interface;
using LittleSword.Player.Controllers;
using Logger = LittleSword.Common.Logger;

namespace LittleSword.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(InputHandler), typeof(CapsuleCollider2D))]
    public class BasePlayer : MonoBehaviour, IDamageable
    {
        // Components 캐싱 변수
        private Rigidbody2D rb;
        private SpriteRenderer spriteRenderer;
        private Animator animator;
        
        // Controller 설정
        private InputHandler inputHandler;
        private MovementController movementController;
        private AnimationController animationController;
        
        // 플레이어 스탯
        [SerializeField] private PlayerStats playerStats;
        
        // 프로퍼티
        public bool IsDead => CurrentHP <= 0;
        public int CurrentHP { get; set; }
        
        #region 유니티 이벤트
        protected virtual void Awake()
        {
            InitComponents();
            InitController();
        }

        protected virtual void OnEnable()
        {
            inputHandler.OnMove += Move;
            inputHandler.OnAttack += Attack;
        }

        protected virtual void OnDisable()
        {
            inputHandler.OnMove -= Move;
            inputHandler.OnAttack -= Attack;
        }

        #endregion
        
        #region 초기화 로직
        private void InitComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
            rb.freezeRotation = false;
            
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            
            // 플레이어 HP 초기화
            CurrentHP = playerStats.maxHP;
        }
        
        private void InitController()
        {
            inputHandler = GetComponent<InputHandler>();
            movementController = new MovementController(rb, spriteRenderer);
            animationController = new AnimationController(animator);
        }
        #endregion
        
        #region 공통 메소드
        private void Move(Vector2 direction)
        {
            movementController.Move(direction, playerStats.moveSpeed);
            animationController.Move(direction != Vector2.zero);
            Logger.Log("이동 처리");
        }
        
        private void Attack()
        {
            animationController.Attack();
            Logger.Log("공격 로직");
        }

        public void TakeDamage(int damage)
        {
            CurrentHP = Mathf.Max(0, CurrentHP - damage);
            if (IsDead)
                Die();
            else
                animationController.Hit();   
        }

        public void Die()
        {
            Logger.Log("사망");
            animationController.Die();
        }
        #endregion
    }    
}
