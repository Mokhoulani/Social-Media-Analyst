import { createNativeStackNavigator } from '@react-navigation/native-stack'
import React from 'react'
import { AuthFacade } from '../store/auth/facade'
import AuthTabs from './AuthTabsNavigator'
import BottomTabsNavigator from './BottomTabsNavigator'

export type RootStackParamList = {
    HomeScreen: undefined
    SettingScreen: undefined
}

const RootStack = createNativeStackNavigator<RootStackParamList>()

export default function RootNavigator() {
    const isAuthenticated = AuthFacade.isAuthenticated()

    return (
        <RootStack.Navigator screenOptions={{ headerShown: false }}>
            {isAuthenticated ? (
                <RootStack.Screen
                    name="HomeScreen"
                    component={BottomTabsNavigator}
                />
            ) : (
                <RootStack.Screen name="HomeScreen" component={AuthTabs} />
            )}
        </RootStack.Navigator>
    )
}
