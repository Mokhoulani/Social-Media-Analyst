import { createNativeStackNavigator } from '@react-navigation/native-stack'
import React from 'react'
import { useAuthCheck } from '../hooks/useAuthCheck'
import { useDeviceId } from '../hooks/useDeviceId'
import { useRegisterPushToken } from '../hooks/useRegisterPushToken'
import AuthTabs from './AuthTabsNavigator'
import BottomTabsNavigator from './BottomTabsNavigator'

export type RootStackParamList = {
    HomeScreen: undefined
    SettingScreen: undefined
}

const RootStack = createNativeStackNavigator<RootStackParamList>()

export default function RootNavigator() {
    const isAuthenticated = useAuthCheck()
    useDeviceId()
    useRegisterPushToken(isAuthenticated)

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
