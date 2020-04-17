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

    [Header("Skill Buttons")]
    public Button[] SkillButtons;

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
    [SerializeField] private SceneName sceneToLoad;

    private int currentAction;
    private EventSystem eventSystem;

    private bool attacking;
    private bool usingSkill;

    private bool arenShifted;
    private List<int> hostilityMeter;
    private Skill skillToUse;

    private bool useFirstSkill;
    private bool useSecondSkill;

    // Start is called before the first frame update
    void Start() {
        //GameManager.Instance.InBattle = true;
        eventSystem = EventSystem.current;
        attacking = false;
        usingSkill = false;
        arenShifted = false;
        hostilityMeter = new List<int> { 0 };
        skillToUse = null;
        useFirstSkill = false;
        useSecondSkill = false;

        currentAction = 0;
        updateAction();
    }

    // Update is called once per frame
    void Update() {
        if (!didTutorialEnd() && Input.GetButtonDown("Interact") && TextHud.IsActive()) {
            // if tutorial hasn't ending and has text displayed
            updateCurrentAction();
        } else if (didTutorialEnd() && Input.GetButtonDown("Interact")) {
            // wait to load scene
            //GameManager.Instance.InBattle = false;
            LoadSceneLogic.Instance.LoadScene(sceneToLoad.GetSceneName());
        }
    }

    /// <summary>
    /// Select a target
    /// </summary>
    public void Select() {
        playClick();
        if (attacking) {
            // is attacking
            int damage = BattleLogic.RegularAttack(charName, Enemy);

            // show damage
            displayDamageToEnemies(damage);

            Enemy.Highlight(false);
            attacking = false;
        } else if (usingSkill) {
            // is using skills
            int damage = BattleLogic.UseAttackSkill(charName, Enemy, skillToUse);

            // show damage
            displayDamageToEnemies(damage);

            Enemy.Highlight(false);

            usingSkill = false;
            skillToUse = null;
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
    }

    /// <summary>
    /// Select the Skills command
    /// </summary>
    public void SelectSkills() {
        playClick();

        // turn off main hud
        MainHud.interactable = false;
        MainHud.gameObject.SetActive(false);

        // turn on skill hud
        SkillHud.gameObject.SetActive(true);
        SkillHud.interactable = true;
        DescriptionPanel.SetActive(true);

        if (charName == Constants.AREN) {
            SkillHudUI.OpenSkillsHud(charName, arenShifted);
        } else {
            SkillHudUI.OpenSkillsHud(charName);
        }

        SkillButtons[0].interactable = useFirstSkill;
        SkillButtons[1].interactable = useSecondSkill;

        eventSystem.SetSelectedGameObject(
            SkillButtons[0].interactable ? 
            SkillButtons[0].gameObject : 
            SkillButtons[1].gameObject
        );
    }

    /// <summary>
    /// Select a skill to sue
    /// </summary>
    /// <param name="skill">Skill to use</param>
    public void UseSkill(int skill) {
        skillToUse = SkillHudUI.GetClickedSkill(skill);
        bool endTurn = false;

        if (skillToUse is TauntSkill) {
            playClick();

            hostilityMeter.Add(0);
            hostilityMeter.Add(0);

            skillToUse = null;
            updateCurrentAction();
        } else if ((GameManager.Instance.Party.GetCharCurrSP(charName) > skillToUse.Cost && skillToUse is ShiftSkill) || skillToUse is UnshiftSkill) {
            // aren shifting
            playClick();

            ShiftImage.gameObject.SetActive(skillToUse is ShiftSkill);
            arenShifted = skillToUse is ShiftSkill;
            float magicMeter = GameManager.Instance.GetMagicMeter();
            skillToUse.UseSkill(charName);
            SkillHudUI.OpenSkillsHud(charName, arenShifted);
            skillToUse = null;
            updateCurrentAction();
        } else if (GameManager.Instance.Party.GetCharCurrSP(charName) > skillToUse.Cost) {
            // regular skill
            // can only use if have enough sp to use
            playClick();

            if (skillToUse.IsPhyAttk || skillToUse.IsMagAttk) {
                // use on enemy
                SkillHud.interactable = false;
                SelectHud.gameObject.SetActive(true);
                SelectHud.interactable = true;

                SelectHudUI.OpenSelectHud(new Enemy[] { Enemy });
            }

            usingSkill = true;
        }
        if (endTurn) {
            SkillHud.gameObject.SetActive(false);
            SkillHud.interactable = false;
            DescriptionPanel.SetActive(false);
            SkillHudUI.ExitSkillHud();
            CharTurnImage.gameObject.SetActive(false);
            updateCurrentAction();
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

    /// <summary>
    /// Update the current action and call updateAction
    /// </summary>
    private void updateCurrentAction() {
        currentAction++;
        updateAction();
    }

    /// <summary>
    /// Update the Battle hud according to the current action
    /// </summary>
    private void updateAction() {
        closeEverything();
        if (didTutorialEnd()) {
            // end of battle because no more actions
            endBattle();
        } else {
            // update for the next action
            TutorialAction currAction = tutorialActions[currentAction];

            if (currAction.GetText().Length > 0) {
                // if text is not empty string, show text
                showText(currAction.GetText());
            } else if (currAction.GetAttackInteractable() || currAction.GetSkillsInteractable()) {
                // if player is attacking or doing skills
                setMainCommands(currAction.GetAttackInteractable(), currAction.GetSkillsInteractable());
                useFirstSkill = currAction.GetUseFirstSkill();
                useSecondSkill = currAction.GetUseSecondSkill();
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
        }
        Instantiate(DamageNumber).SetDamage(
            CharPos.transform.position,
            enemyTurn.Amount,
            true
        );
    }

    /// <summary>
    /// Get whether or not the tutorial has ended
    /// </summary>
    /// <returns>True if the tutorial has ended, false otherwise</returns>
    private bool didTutorialEnd() {
        return currentAction >= tutorialActions.Length;
    }

    /// <summary>
    /// Handles the end of tutorial battle
    /// </summary>
    private void endBattle() {
        GameManager.Instance.Party.ResetStatsModifier();
        GameManager.Instance.Party.GainExperience(Enemy.Exp);
        GameManager.Instance.GainMoney(Enemy.Money);
        GameManager.Instance.Party.RecoverParty();

        ShiftImage.gameObject.SetActive(false);
        StartCoroutine(enemyDied());
        showText(
            "The enemy ran away! You earned " + Enemy.Exp + " exp and " + Enemy.Money + " money!"
        );
    }

    /// <summary>
    /// Wait for enemy to die and then remove the enemy
    /// </summary>
    /// <param name="enemy">Enemy who died</param>
    /// <param name="choice">THe index of the enemy in the enemy list</param>
    /// <returns></returns>
    private IEnumerator enemyDied() {
        Enemy.CurrentHP -= 9999;
        yield return new WaitForSeconds(1f);
    }

    /// <summary>
    /// Close all the HUDs
    /// </summary>
    private void closeEverything() {
        TextImage.SetActive(false);
        TextHud.gameObject.SetActive(false);

        MainHud.gameObject.SetActive(false);
        MainHud.interactable = false;

        CharTurnImage.gameObject.SetActive(false);

        SelectHud.gameObject.SetActive(false);
        SelectHud.interactable = false;

        SkillHud.gameObject.SetActive(false);
        SkillHud.interactable = false;
        SkillHudUI.ExitSkillHud();

        DescriptionPanel.SetActive(false);
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
