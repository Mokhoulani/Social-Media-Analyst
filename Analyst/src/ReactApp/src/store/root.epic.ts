import { Epic, combineEpics } from 'redux-observable'
import { Action } from 'typesafe-actions'
import { authEpics } from './auth/epic'
import { deviceEpics } from './device/epic'
import { goalEpics } from './goal/epic'
import { platformEpics } from './platform/epic'
import { RootState } from './root.reducer'
import { Dependencies } from './store'
import { usageEpics } from './usage/epict'
import { userEpics } from './user/epic'

export const rootEpic: Epic<Action, Action, RootState, Dependencies> =
    combineEpics(
        ...(authEpics as Epic<Action, Action, RootState, Dependencies>[]),
        ...(userEpics as Epic<Action, Action, RootState, Dependencies>[]),
        ...(deviceEpics as Epic<Action, Action, RootState, Dependencies>[]),
        ...(platformEpics as Epic<Action, Action, RootState, Dependencies>[]),
        ...(goalEpics as Epic<Action, Action, RootState, Dependencies>[]),
        ...(usageEpics as Epic<Action, Action, RootState, Dependencies>[])
    )
