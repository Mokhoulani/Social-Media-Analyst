import { combineReducers } from 'redux'
import { authReducer } from './auth/reducer'
import { deviceReducer } from './device/reducer'
import { goalReducer } from './goal/reducer'
import { platformReducer } from './platform/reducer'
import { themeReducer } from './theme/reducer'
import { userReducer } from './user/reducer'

// Combine all your reducers into one rootReducer
export const rootReducer = combineReducers({
    auth: authReducer,
    user: userReducer,
    theme: themeReducer,
    device: deviceReducer,
    platforms: platformReducer,
    goal: goalReducer,
})

export type RootState = {
    auth: ReturnType<typeof authReducer>
    user: ReturnType<typeof userReducer>
    theme: ReturnType<typeof themeReducer>
    device: ReturnType<typeof deviceReducer>
    platforms: ReturnType<typeof platformReducer>
    goal: ReturnType<typeof goalReducer>
}
