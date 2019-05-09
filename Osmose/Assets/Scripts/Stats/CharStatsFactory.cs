public static class CharStatsFactory {
    public static CharStats GeneratorChar(string name) {
        CharStats charStats = null;
        Skill[] skills = SkillsManager.Instance.GetCharSkills(name);
        switch (name) {
            case Constants.AREN:
                charStats = new CharStats(145, 25, 50, 70, 25, 60, 15, skills);
                break;
            case Constants.REY:
                charStats = new CharStats(125, 50, 35, 40, 50, 45, 20, skills);
                break;
            case Constants.NAOISE:
                charStats = new CharStats(200, 35, 20, 30, 70, 50, 30, skills);
                break;
        }
        return charStats;
    }
}
