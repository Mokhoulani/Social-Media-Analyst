import { createMaterialTopTabNavigator } from '@react-navigation/material-top-tabs'
import React from 'react'
import { useWindowDimensions } from 'react-native'

type TabRoute<T extends string> = {
    name: T
    component: React.ComponentType<unknown>
    options?: object
}

type Props<T extends string> = {
    routes: TabRoute<T>[]
}

export function createGenericTopTabsNavigator<T extends string>() {
    const Tab = createMaterialTopTabNavigator<Record<T, undefined>>()

    return function GenericTopTabsNavigator({ routes }: Props<T>) {
        const { width } = useWindowDimensions()

        return (
            <Tab.Navigator
                screenOptions={{
                    tabBarStyle: {
                        backgroundColor: '#fff',
                        elevation: 0,
                        shadowOpacity: 0,
                    },
                    tabBarIndicatorStyle: {
                        backgroundColor: '#007AFF',
                        height: 3,
                    },
                    tabBarLabelStyle: {
                        fontSize: 12,
                    },
                    tabBarActiveTintColor: '#007AFF',
                    tabBarInactiveTintColor: '#666',
                    tabBarPressColor: '#E9E9E9',
                    tabBarScrollEnabled: width < 768,
                    lazy: true,
                    lazyPlaceholder: () => null,
                }}
            >
                {routes.map(({ name, component, options }) => (
                    <Tab.Screen
                        key={name}
                        name={name}
                        component={component}
                        options={options}
                    />
                ))}
            </Tab.Navigator>
        )
    }
}
