import React, { useState } from 'react'
import { Button, Modal, StyleSheet, Text, TextInput, View } from 'react-native'
import { SocialMediaPlatform } from '../../entities/SocialMediaPlatform'

interface Props {
    visible: boolean
    platform: SocialMediaPlatform | null
    onClose: () => void
    onSaveGoal: (platformId: number, seconds: number) => void
}

export const GoalSetterModal: React.FC<Props> = ({
    visible,
    platform,
    onClose,
    onSaveGoal,
}) => {
    const [minutes, setMinutes] = useState('60')

    if (!platform) return null

    return (
        <Modal visible={visible} transparent animationType="slide">
            <View style={styles.modal}>
                <Text style={styles.title}>
                    Set Daily Limit for {platform.name}
                </Text>

                <TextInput
                    keyboardType="numeric"
                    value={minutes}
                    onChangeText={setMinutes}
                    style={styles.input}
                />
                <Text>Minutes per day</Text>

                <View style={styles.buttons}>
                    <Button title="Cancel" onPress={onClose} />
                    <Button
                        title="Save"
                        onPress={() => {
                            const seconds = parseInt(minutes) * 60
                            onSaveGoal(platform.id, seconds)
                            onClose()
                        }}
                    />
                </View>
            </View>
        </Modal>
    )
}

const styles = StyleSheet.create({
    modal: {
        marginTop: '50%',
        marginHorizontal: 16,
        backgroundColor: 'white',
        borderRadius: 12,
        padding: 20,
        elevation: 6,
    },
    title: {
        fontSize: 18,
        fontWeight: '600',
        marginBottom: 12,
    },
    input: {
        borderBottomWidth: 1,
        marginBottom: 12,
        fontSize: 16,
        padding: 4,
    },
    buttons: {
        flexDirection: 'row',
        justifyContent: 'space-between',
    },
})
