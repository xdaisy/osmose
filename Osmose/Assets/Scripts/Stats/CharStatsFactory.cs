public static class CharStatsFactory {
    public static CharStats GeneratorChar(string name) {
        CharStats charStats = null;
        Skill[] skills = SkillsManager.Instance.GetCharSkills(name);
        Stats stats = StatsManager.Instance.GetCharStatsAt(name, 1);
        charStats = new CharStats(
            name,
            stats.HP,
            stats.SP,
            stats.Attack,
            stats.Defense,
            stats.MagicDefense,
            stats.Speed,
            stats.Luck,
            skills
        );
        return charStats;
    }
}
