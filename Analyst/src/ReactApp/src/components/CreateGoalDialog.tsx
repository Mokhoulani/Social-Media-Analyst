import { zodResolver } from '@hookform/resolvers/zod'
import React, { useCallback } from 'react'
import { Controller, useForm } from 'react-hook-form'
import {
    ActivityIndicator,
    Alert,
    Modal,
    SafeAreaView,
    Text,
    TextInput,
    TouchableOpacity,
    View,
} from 'react-native'
import { Dropdown } from 'react-native-element-dropdown'
import { useSelector } from 'react-redux'
import { SocialMediaPlatform } from '../entities/SocialMediaPlatform'
import { UserUsageGoal, UserUsageGoalSchema } from '../entities/UserUsageGoal'
import { userSelectors } from '../store/user/selectors'
import { useStyles } from '../themes/useStyles'

interface Props {
    visible: boolean
    onClose: () => void
    onSubmit: (data: UserUsageGoal) => void
    platforms: SocialMediaPlatform[] | null
}

export const CreateGoalDialog: React.FC<Props> = ({
    visible,
    onClose,
    onSubmit,
    platforms,
}) => {
    const user = useSelector(userSelectors.selectUser)
    const styles = useStyles()

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
    } = useForm<UserUsageGoal>({
        resolver: zodResolver(UserUsageGoalSchema),
        defaultValues: {
            id: 0,
            userId: user?.id,
            platformId: 0,
            dailyLimit: '',
        },
    })

    const handleFormSubmit = useCallback(
        (data: UserUsageGoal) => {
            try {
                console.log(data)
                onSubmit(data)
                reset()
                onClose()
            } catch (error) {
                console.error('Error submitting form:', error)
                Alert.alert('Error', 'Failed to create goal. Please try again.')
            }
        },
        [onSubmit, reset, onClose]
    )

    const handleModalClose = useCallback(() => {
        reset()
        onClose()
    }, [reset, onClose])

    // Transform platforms data for dropdown
    const dropdownData =
        platforms?.map((p) => ({
            label: p.name,
            value: p.id,
        })) || []

    return (
        <Modal
            visible={visible}
            animationType="slide"
            transparent={false}
            onRequestClose={handleModalClose}
        >
            <SafeAreaView style={styles.modalContainer}>
                <View style={styles.container}>
                    <View style={styles.header}>
                        <Text style={styles.title}>Create Usage Goal</Text>
                        <TouchableOpacity
                            onPress={handleModalClose}
                            style={styles.closeButton}
                            disabled={isSubmitting}
                        >
                            <Text style={styles.closeButtonText}>×</Text>
                        </TouchableOpacity>
                    </View>

                    <View style={styles.content}>
                        <Text style={styles.label}>
                            Set Daily Limit (in minutes)
                        </Text>
                        <Controller
                            control={control}
                            name="dailyLimit"
                            render={({
                                field: { onChange, onBlur, value },
                            }) => (
                                <TextInput
                                    keyboardType="numbers-and-punctuation" // Allows colon input
                                    onChangeText={(val) => {
                                        onChange(val) // Directly store the hh:mm:ss string
                                    }}
                                    onBlur={onBlur}
                                    value={value ?? ''}
                                    style={styles.input}
                                    placeholder="Enter time (hh:mm:ss)"
                                />
                            )}
                        />
                        {errors.dailyLimit && (
                            <Text style={styles.error}>
                                {errors.dailyLimit.message}
                            </Text>
                        )}

                        <Text style={styles.label}>Select Platform</Text>
                        {!platforms || platforms.length === 0 ? (
                            <ActivityIndicator style={styles.loading} />
                        ) : (
                            <Controller
                                control={control}
                                name="platformId"
                                render={({ field: { value, onChange } }) => (
                                    <Dropdown
                                        style={styles.dropdown}
                                        placeholderStyle={
                                            styles.placeholderStyle
                                        }
                                        selectedTextStyle={
                                            styles.selectedTextStyle
                                        }
                                        data={dropdownData}
                                        maxHeight={300}
                                        labelField="label"
                                        valueField="value"
                                        placeholder="Select a platform"
                                        value={value === 0 ? null : value}
                                        onChange={(item) => {
                                            onChange(item.value)
                                        }}
                                        disable={isSubmitting}
                                    />
                                )}
                            />
                        )}
                        {errors.platformId && (
                            <Text style={styles.error}>
                                {errors.platformId.message}
                            </Text>
                        )}
                    </View>

                    <View style={styles.buttonContainer}>
                        <TouchableOpacity
                            style={[
                                styles.primaryButton,
                                (isSubmitting || !platforms?.length) &&
                                    styles.disabledButton,
                            ]}
                            onPress={handleSubmit(handleFormSubmit)}
                            disabled={isSubmitting || !platforms?.length}
                        >
                            <Text style={styles.primaryButtonText}>
                                {isSubmitting ? 'Creating...' : 'Create Goal'}
                            </Text>
                        </TouchableOpacity>

                        <TouchableOpacity
                            style={[
                                styles.secondaryButton,
                                isSubmitting && styles.disabledButton,
                            ]}
                            onPress={handleModalClose}
                            disabled={isSubmitting}
                        >
                            <Text style={styles.secondaryButtonText}>
                                Cancel
                            </Text>
                        </TouchableOpacity>
                    </View>
                </View>
            </SafeAreaView>
        </Modal>
    )
}
