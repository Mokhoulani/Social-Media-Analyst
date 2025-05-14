import { createReducer } from '@reduxjs/toolkit';
import { ThemeActions } from './actions';


export type ColorMode = 'light' | 'dark' | 'auto';

export type ThemeState = {
    colorMode: ColorMode;
};

export const initialState: ThemeState = {
    colorMode: 'auto',
};

// Reducer using builder notation
export const themeReducer = createReducer(initialState, (builder) => {
    builder.addCase(ThemeActions.setColorMode, (state, action) => {
        state.colorMode = action.payload.colorMode;
    });
});
