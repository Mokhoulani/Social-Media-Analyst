import MaterialIcons from '@expo/vector-icons/MaterialIcons'
import { createBottomTabNavigator } from '@react-navigation/bottom-tabs'
import React from 'react'
import SettingsScreen from '../screens/SettingsScreen'
import MeterialTopTabsNavigator from './MaterialTopTabsNavigator'

export type TabParamsList = {
    Home: undefined
    Settings: undefined
}

const Tab = createBottomTabNavigator<TabParamsList>()

export default function BottomTabsNavigator() {
    return (
        <Tab.Navigator screenOptions={{ headerShown: false }}>
            <Tab.Screen
                name="Home"
                component={MeterialTopTabsNavigator}
                options={{
                    tabBarIcon({ size, color }) {
                        return (
                            <MaterialIcons
                                name="home"
                                size={size}
                                color={color}
                            />
                        )
                    },
                }}
            />
            <Tab.Screen
                name="Settings"
                component={SettingsScreen}
                options={{
                    tabBarIcon({ size, color }) {
                        return (
                            <MaterialIcons
                                name="settings"
                                size={size}
                                color={color}
                            />
                        )
                    },
                }}
            />
        </Tab.Navigator>
    )
}
