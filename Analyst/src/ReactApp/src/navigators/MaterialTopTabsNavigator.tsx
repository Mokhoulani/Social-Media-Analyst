import { createMaterialTopTabNavigator } from '@react-navigation/material-top-tabs'
import { useWindowDimensions } from 'react-native'
import { useTheme } from 'react-native-paper'
import { HomeScreen } from '../screens/HomeScreen'
import { LoginScreen } from '../screens/LoginScreen'
import SetGoalScreen from '../screens/SetGoalScreen'

export type TabAuthParamsList = {
    signIn: undefined
    signUp: undefined
    Goal: undefined
}

const Tab = createMaterialTopTabNavigator<TabAuthParamsList>()

export default function MeterialTopTabsNavigator() {
    const { colors } = useTheme()
    const { width } = useWindowDimensions()
    return (
        <Tab.Navigator
            screenOptions={{
                tabBarStyle: {
                    backgroundColor: colors.surface,
                    elevation: 0,
                    shadowOpacity: 0,
                },
                tabBarIndicatorStyle: {
                    backgroundColor: colors.primary,
                    height: 3,
                },
                tabBarLabelStyle: {
                    fontSize: 12,
                },
                tabBarActiveTintColor: colors.primary,
                tabBarInactiveTintColor: colors.onSurfaceVariant,
                tabBarPressColor: colors.surfaceVariant,
                tabBarScrollEnabled: width < 768,
                lazy: true,
                lazyPlaceholder: () => null,
            }}
        >
            <Tab.Screen name="signUp" component={HomeScreen} />
            <Tab.Screen name="signIn" component={LoginScreen} />
            <Tab.Screen name="Goal" component={SetGoalScreen} />
        </Tab.Navigator>
    )
}
