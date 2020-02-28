using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Class that handles the battle tutorials
/// </summary>
public class BattleTutorial : MonoBehaviour {
    [Header("HUDs")]
    public CanvasGroup MainHud;
    public CanvasGroup SkillHud;
    public CanvasGroup SelectHud;
    public GameObject TextImage;
    public Text TextHud;
    public GameObject DescriptionPanel;

    [Header("Hud UI Objects")]
    public SelectHud SelectHudUI;
    public SkillHud SkillHudUI;

    [Header("Main HUD Buttons")]
    public Button AttackButton;
    public Button SkillsButton;

    [Header("Enemy")]
    public Enemy Enemy;

    [Header("Character Pos")]
    public Transform CharPos;
    public Image CharTurnImage;
    public Image ShiftImage;

    [Header("Effects")]
    public DamageEffect DamageNumber;

    [SerializeField] string charName;
    [SerializeField] private TutorialAction[] tutorialActions;

    private int currentAction;
    private EventSystem eventSystem;

    private GameObject prevButton;
    private bool attacking;

    private List<int> hostilityMeter;

    // Start is called before the first frame update
    void Start() {
        GameManager.Instance.InBattle = true;
        eventSystem = EventSystem.current;
        attacking = false;
        hostilityMeter = new List<int> { 0 };

        currentAction = 0;
        updateAction();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetButtonDown("Interact") && TextHud.IsActive()) {
            updateCurrentAction();
        }
    }

    /// <summary>
    /// Choose to attack
    /// </summary>
    public void SelectAttack() {
        playClick();

        MainHud.interactable = false;
        SelectHud.gameObject.SetActive(true);
        SelectHud.interactable = true;

        SelectHudUI.OpenSelectHud(new Enemy[] { Enemy });

        attacking = true;

        prevButton = EventSystem.current.currentSelectedGameObject;
    }

    public void Select() {
        playClick();
        if (attacking) {
            int damage = BattleLogic.RegularAttack(charName, Enemy);

            // show damage
            displayDamageToEnemies(damage);

            Enemy.Highlight(false);
            attacking = false;
        }
    }

    /// <summary>
    /// Display the damage dealt to enemy
    /// </summary>
    /// <param name="damage">Amount of damage done to enemy</param>
    private void displayDamageToEnemies(int damage) {
        StartCoroutine(showDamage(Enemy.transform.position, damage));
    }

    /// <summary>
    /// Show the damage amount for player and enemy if enemy didn't die or recovery amount
    /// </summary>
    /// <param name="pos">Position of the character getting attacked/healed</param>
    /// <param name="amount">Amount of damage or hit point healed</param>
    /// <returns></returns>
    private IEnumerator showDamage(Vector3 pos, int amount) {
        playDamageSound();

        Instantiate(DamageNumber).SetDamage(pos, amount, true);
        yield return new WaitForSeconds(DamageNumber.Duration);
        updateCurrentAction();
    }

    private void updateCurrentAction() {
        currentAction++;
        updateAction();
    }

    /// <summary>
    /// Update the Battle hud according to the current action
    /// </summary>
    private void updateAction() {
        if (currentAction >= tutorialActions.Length) {
            // end of battle because no more actions
            endBattle();
        } else {
            // update for the next action
            TutorialAction currAction = tutorialActions[currentAction];
            closeEverything();

            if (currAction.GetText().Length > 0) {
                // if text is not empty string, show text
                showText(currAction.GetText());
            } else if (currAction.GetAttackInteractable() || currAction.GetSkillsInteractable()) {
                // if player is attacking or doing skills
                setMainCommands(currAction.GetAttackInteractable(), currAction.GetSkillsInteractable());
            } else {
                // enemy's turn
                StartCoroutine(enemyAttackCo());
            }
        }
    }

    /// <summary>
    /// Display text
    /// </summary>
    /// <param name="text">Text to display</param>
    private void showText(string text) {
        TextImage.SetActive(true);
        TextHud.gameObject.SetActive(true);
        TextHud.text = text;
    }

    /// <summary>
    /// Set the command buttons interaction depending on parameters passed in
    /// </summary>
    /// <param name="attackInteractable">Flag for whether or not can attack</param>
    /// <param name="skillsInteractable">Flag for whether or not can use skills</param>
    private void setMainCommands(bool attackInteractable, bool skillsInteractable) {
        // show main hud and make interactable
        MainHud.gameObject.SetActive(true);
        MainHud.interactable = true;

        // show character turn image
        CharTurnImage.gameObject.SetActive(true);

        // make command buttons interactable depending on if interactable or not
        AttackButton.interactable = attackInteractable;
        SkillsButton.interactable = skillsInteractable;

        if (attackInteractable) {
            // set attack as selected button if can attack
            eventSystem.SetSelectedGameObject(AttackButton.gameObject);
        } else {
            // else set skills as selected button
            eventSystem.SetSelectedGameObject(SkillsButton.gameObject);
        }
    }

    /// <summary>
    /// Play animation for enemy's turn
    /// </summary>
    /// <param name="enemy">Enemy whose turn it is</param>
    /// <returns></returns>
    private IEnumerator enemyAttackCo() {
        Enemy.DoMove();
        yield return new WaitForSeconds(0.5f);
        enemyAttack();
        yield return new WaitForSeconds(0.5f);
        updateCurrentAction();
    }

    /// <summary>
    /// Determine what the enemy does during its turn
    /// </summary>
    private void enemyAttack() {
        EnemyTurn enemyTurn = BattleLogic.EnemyTurn(Enemy, hostilityMeter);
        while (!enemyTurn.Attack) {
            // keep calling until enemy is attacking
            enemyTurn = BattleLogic.EnemyTurn(Enemy, hostilityMeter);
            Instantiate(DamageNumber).SetDamage(
                CharPos.transform.position,
                enemyTurn.Amount, 
                true
            );
        }
    }

    private void endBattle() {
        Debug.Log("end of battle");
    }

    private void closeEverything() {
        TextImage.SetActive(false);
        TextHud.gameObject.SetActive(false);

        MainHud.gameObject.SetActive(false);
        MainHud.interactable = false;

        CharTurnImage.gameObject.SetActive(false);

        SelectHud.gameObject.SetActive(false);
        SelectHud.interactable = false;
    }

    /// <summary>
    /// Play click sound effect
    /// </summary>
    private void playClick() {
        SoundManager.Instance.PlaySFX(0);
    }

    /// <summary>
    /// Play damaged sound effect
    /// </summary>
    private void playDamageSound() {
        SoundManager.Instance.PlaySFX(0);
    }

    /// <summary>
    /// Play sound effects for battling
    /// </summary>
    private void playBattleSound() {
        SoundManager.Instance.PlaySFX(0);
    }
}
