import { Action, configureStore, Middleware } from '@reduxjs/toolkit'
import { createEpicMiddleware } from 'redux-observable'
import { rootEpic } from './root-epic'
import { rootReducer, RootState } from './root-reducer'

export type Dependencies = object

// Create the epic middleware with proper types
const epicMiddleware = createEpicMiddleware<
    Action,
    Action,
    RootState,
    Dependencies
>()

export const store = configureStore({
    reducer: rootReducer,
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware().concat(epicMiddleware as Middleware),
})

epicMiddleware.run(rootEpic)

export type AppDispatch = typeof store.dispatch
