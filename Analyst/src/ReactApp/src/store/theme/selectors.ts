import { RootState } from '../root.reducer';
// Selector to get the entire theme state
export const selectThemeState = (state: RootState) => state.theme.colorMode;

// Selector to get the color mode from the theme state
export const selectColorMode = (state: RootState) => state.theme.colorMode;
