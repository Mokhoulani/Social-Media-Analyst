import { createAction } from '@reduxjs/toolkit'

export const ThemeActions = {
    setColorMode: createAction<{ colorMode: 'light' | 'dark' | 'auto' }>(
        'theme/setColorMode'
    ),
}
