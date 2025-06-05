import { createMaterialTopTabNavigator } from '@react-navigation/material-top-tabs'
import { useWindowDimensions } from 'react-native'
import { useTheme } from 'react-native-paper'
import DailyUsageScreen from '../screens/DailyUsageScreen'
import SetGoalScreen from '../screens/SetGoalScreen'

export type TabAuthParamsList = {
    Daily: undefined
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
            <Tab.Screen name="Goal" component={SetGoalScreen} />
            <Tab.Screen name="Daily" component={DailyUsageScreen} />
        </Tab.Navigator>
    )
}
