import React, { useState } from 'react'
import {
    ActivityIndicator,
    Alert,
    ScrollView,
    StyleSheet,
    Text,
    TouchableOpacity,
    View,
} from 'react-native'
import { useAppUsageService } from '../hooks/useAppUsageService'

export default function ServiceControlScreen() {
    const { hasPermission, isTracking, start, stop, requestPermission } =
        useAppUsageService()
    const [isLoading, setIsLoading] = useState(false)

    const handleStartService = async () => {
        setIsLoading(true)
        try {
            if (!hasPermission) {
                setIsLoading(false)
                Alert.alert(
                    'Permission Required',
                    'This app needs usage access permission to track app usage. Please grant permission in the next screen.',
                    [
                        { text: 'Cancel', style: 'cancel' },
                        { text: 'Open Settings', onPress: requestPermission },
                    ]
                )
                return
            }

            const success = await start()
            if (success) {
                Alert.alert('Success', 'Usage tracking service started!')
            } else {
                Alert.alert('Failed', 'Failed to start the tracking service.')
            }
        } finally {
            setIsLoading(false)
        }
    }

    const handleStopService = async () => {
        setIsLoading(true)
        try {
            const success = await stop()
            if (true) {
                Alert.alert('Success', 'Usage tracking service stopped!')
            } else {
                Alert.alert('Failed', 'Failed to stop the tracking service.')
            }
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <ScrollView style={styles.container}>
            <View style={styles.header}>
                <Text style={styles.title}>App Usage Tracker</Text>
                <Text style={styles.subtitle}>
                    Monitor how much time you spend on apps.
                </Text>
            </View>

            <View style={styles.statusCard}>
                <Text style={styles.statusTitle}>Service Status</Text>
                <View style={styles.statusRow}>
                    <Text style={styles.statusLabel}>Permission Granted</Text>
                    <Text
                        style={[
                            styles.statusValue,
                            hasPermission ? styles.success : styles.error,
                        ]}
                    >
                        {hasPermission ? 'Yes' : 'No'}
                    </Text>
                </View>
                <View style={styles.statusRow}>
                    <Text style={styles.statusLabel}>Service Running</Text>
                    <Text
                        style={[
                            styles.statusValue,
                            isTracking ? styles.success : styles.error,
                        ]}
                    >
                        {isTracking ? 'Yes' : 'No'}
                    </Text>
                </View>
            </View>

            <View style={styles.controls}>
                {!hasPermission && (
                    <TouchableOpacity
                        style={styles.permissionButton}
                        onPress={requestPermission}
                        disabled={isLoading}
                    >
                        {isLoading ? (
                            <ActivityIndicator color="#fff" />
                        ) : (
                            <Text style={styles.buttonText}>
                                Grant Usage Permission
                            </Text>
                        )}
                    </TouchableOpacity>
                )}

                <TouchableOpacity
                    style={[
                        styles.button,
                        isTracking ? styles.stopButton : styles.startButton,
                    ]}
                    onPress={
                        isTracking ? handleStopService : handleStartService
                    }
                    disabled={isLoading}
                >
                    {isLoading ? (
                        <ActivityIndicator color="#fff" />
                    ) : (
                        <Text style={styles.buttonText}>
                            {isTracking ? 'Stop Tracking' : 'Start Tracking'}
                        </Text>
                    )}
                </TouchableOpacity>
            </View>

            <View style={styles.infoCard}>
                <Text style={styles.infoTitle}>Why We Track</Text>
                <Text style={styles.infoText}>
                    This app helps you become more aware of your app usage
                    habits by monitoring how much time you spend on different
                    apps. We respect your privacy and only store data on your
                    device unless you explicitly choose to sync.
                </Text>
            </View>
        </ScrollView>
    )
}

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#f5f5f5',
    },
    header: {
        padding: 20,
        backgroundColor: '#fff',
        marginBottom: 10,
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
        color: '#333',
        marginBottom: 5,
    },
    subtitle: {
        fontSize: 16,
        color: '#666',
    },
    statusCard: {
        backgroundColor: '#fff',
        margin: 10,
        padding: 20,
        borderRadius: 10,
    },
    statusTitle: {
        fontSize: 18,
        fontWeight: 'bold',
        color: '#333',
        marginBottom: 15,
    },
    statusRow: {
        flexDirection: 'row',
        justifyContent: 'space-between',
        marginBottom: 10,
    },
    statusLabel: {
        fontSize: 16,
        color: '#666',
    },
    statusValue: {
        fontSize: 16,
        fontWeight: 'bold',
    },
    success: {
        color: '#4CAF50',
    },
    error: {
        color: '#F44336',
    },
    errorText: {
        color: '#F44336',
        fontSize: 14,
        marginTop: 10,
        fontStyle: 'italic',
    },
    controls: {
        padding: 10,
    },
    button: {
        padding: 15,
        borderRadius: 10,
        alignItems: 'center',
        marginBottom: 10,
    },
    startButton: {
        backgroundColor: '#4CAF50',
    },
    stopButton: {
        backgroundColor: '#F44336',
    },
    permissionButton: {
        backgroundColor: '#2196F3',
        padding: 15,
        borderRadius: 10,
        alignItems: 'center',
        marginBottom: 10,
    },
    buttonText: {
        color: '#fff',
        fontSize: 16,
        fontWeight: 'bold',
    },
    infoCard: {
        backgroundColor: '#fff',
        margin: 10,
        padding: 20,
        borderRadius: 10,
    },
    infoTitle: {
        fontSize: 18,
        fontWeight: 'bold',
        color: '#333',
        marginBottom: 10,
    },
    infoText: {
        fontSize: 16,
        color: '#666',
        lineHeight: 24,
    },
})
