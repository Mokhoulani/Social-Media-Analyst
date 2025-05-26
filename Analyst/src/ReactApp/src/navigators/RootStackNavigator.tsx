import { createNativeStackNavigator } from '@react-navigation/native-stack'
import React from 'react'
import { authSelectors } from '../store/auth/selectors'
import { useAppSelector } from '../store/hook'
import AuthTabs from './AuthTabsNavigator'
import BottomTabsNavigator from './BottomTabsNavigator'

export type RootStackParamList = {
    HomeScreen: undefined
    NavigatorAuth: undefined
}

const RootStack = createNativeStackNavigator<RootStackParamList>()

export default function RootNavigator() {
    const isAuthenticated = useAppSelector(authSelectors.selectIsAuthenticated)

    return (
        <RootStack.Navigator screenOptions={{ headerShown: false }}>
            {isAuthenticated ? (
                <RootStack.Screen
                    name="HomeScreen"
                    component={BottomTabsNavigator}
                />
            ) : (
                <RootStack.Screen name="NavigatorAuth" component={AuthTabs} />
            )}
        </RootStack.Navigator>
    )
}
