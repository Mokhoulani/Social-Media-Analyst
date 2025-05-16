import { combineReducers } from 'redux'
import { authReducer } from './auth/reducer'
import { userReducer } from './user/reducer'
import { themeReducer } from './theme/reducer'

// Combine all your reducers into one rootReducer
export const rootReducer = combineReducers({
    auth: authReducer,
    user: userReducer,
    theme: themeReducer,
})

export type RootState = {
    auth: ReturnType<typeof authReducer>
    user: ReturnType<typeof userReducer>
    theme: ReturnType<typeof themeReducer>
}
