import { Action, configureStore } from '@reduxjs/toolkit'
import { combineEpics, createEpicMiddleware, Epic } from 'redux-observable'
import { authEpics } from './auth/epic'
import { authReducer } from './auth/reducer'

export type RootState = {
    auth: ReturnType<typeof authReducer>
}

export type Dependencies = object

const epicMiddleware = createEpicMiddleware<
    Action,
    Action,
    RootState,
    Dependencies
>({})

export const store = configureStore({
    reducer: {
        auth: authReducer,
    },
    middleware: (getDefaultMiddleware) =>
        getDefaultMiddleware({
            thunk: false,
            serializableCheck: {
                ignoredActions: [
                    'auth/loginRequest',
                    'auth/signUpRequest',
                    'auth/refreshTokenRequest',
                ],
            },
        }).concat(epicMiddleware),
})

export const rootEpic: Epic<Action, Action, RootState, Dependencies> =
    combineEpics(
        ...(authEpics as Epic<Action, Action, RootState, Dependencies>[])
    )

epicMiddleware.run(rootEpic)

export type AppDispatch = typeof store.dispatch
