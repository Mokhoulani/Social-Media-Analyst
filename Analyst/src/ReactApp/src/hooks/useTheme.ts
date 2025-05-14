

export const useTheme = () => {

    const isDark = colorMode === 'dark';

    return getCombinedTheme(avatarTheme, isDark);
};