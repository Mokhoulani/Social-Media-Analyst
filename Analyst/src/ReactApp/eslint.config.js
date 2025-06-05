import js from '@eslint/js'
import jestPlugin from 'eslint-plugin-jest'
import eslintPluginPrettier from 'eslint-plugin-prettier/recommended'
import reactHooks from 'eslint-plugin-react-hooks'
import reactRefresh from 'eslint-plugin-react-refresh'
import globals from 'globals'
import tseslint from 'typescript-eslint'

export default [
    // Ignore folders globally
    {
        ignores: ['dist/', 'obj/', 'node_modules/', '.expo/', '.expo-shared/'],
    },

    // Base JS rules
    js.configs.recommended,

    // TypeScript support
    ...tseslint.configs.recommended,

    // Jest rules for test files
    {
        files: ['**/*.{test,spec}.{js,ts,tsx}'],
        languageOptions: {
            ecmaVersion: 2020,
            globals: {
                ...globals.node,
                ...globals.jest,
            },
        },
        plugins: {
            jest: jestPlugin,
        },
        rules: {
            ...jestPlugin.configs.recommended.rules,
        },
    },

    // Custom rules for TS/TSX files
    {
        files: ['**/*.{ts,tsx}'],
        ignores: ['dist', 'obj'],
        languageOptions: {
            ecmaVersion: 2020,
            globals: globals.browser,
        },
        plugins: {
            'react-hooks': reactHooks,
            'react-refresh': reactRefresh,
        },
        rules: {
            ...reactHooks.configs.recommended.rules,
            'react-refresh/only-export-components': [
                'warn',
                { allowConstantExport: true },
            ],
        },
    },

    // Prettier integration
    eslintPluginPrettier,
    {
        'prettier/prettier': [
            'error',
            {
                singleQuote: true,
                parser: 'flow',
            },
        ],
    },
]
