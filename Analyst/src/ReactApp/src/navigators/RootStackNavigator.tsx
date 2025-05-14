import React from 'react';
import { createNativeStackNavigator } from '@react-navigation/native-stack';
import Homescreen from '../screens/HomeScreen';
import SettingsScreen from '../screens/SettingsScreen';

export type RootStackParamList = {
    Homescreen: undefined;
    Settingscreen: undefined;
};

const RootStack = createNativeStackNavigator<RootStackParamList>();


const isAuthenticated = true;

export default function RootNavigator() {
    return (
        <RootStack.Navigator screenOptions={{ headerShown: false }}>
            {isAuthenticated ? (
                <RootStack.Screen
                    name="Homescreen"
                    component={Homescreen}
                />
            ) : (
                <RootStack.Screen
                    name="Settingscreen"
                    component={SettingsScreen}
                />
            )}
        </RootStack.Navigator>
    );
}
