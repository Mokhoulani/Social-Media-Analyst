import React, { useState } from 'react'
import { View } from 'react-native'
import { PlatformList } from '../components/SocialMedia/PlatformList'
import { GoalSetterModal } from '../components/SocialMedia/GoalSetterModal'
import { useSelector } from 'react-redux'
import { socialMediaSelectors } from '../store/socialMedia/slice'

export default function SetGoalScreen() {
    const platforms = useSelector(socialMediaSelectors.selectAll)
    const [selectedPlatform, setSelectedPlatform] = useState(null)
    const [modalVisible, setModalVisible] = useState(false)

    const handleSelect = (platform) => {
        setSelectedPlatform(platform)
        setModalVisible(true)
    }

    const handleSaveGoal = (platformId, seconds) => {
        // dispatch(UserUsageGoalActions.setGoal({ platformId, seconds }))
        console.log(`Goal set for platform ${platformId}: ${seconds} seconds`)
    }

    return (
        <View>
            <PlatformList
                platforms={platforms}
                onSelectPlatform={handleSelect}
            />
            <GoalSetterModal
                visible={modalVisible}
                platform={selectedPlatform}
                onClose={() => setModalVisible(false)}
                onSaveGoal={handleSaveGoal}
            />
        </View>
    )
}
