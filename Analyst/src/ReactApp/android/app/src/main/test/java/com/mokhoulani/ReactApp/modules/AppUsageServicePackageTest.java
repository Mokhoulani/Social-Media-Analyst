import android.app.NotificationManager;
import android.app.Service;
import android.app.usage.UsageStats;
import android.app.usage.UsageStatsManager;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import androidx.core.app.NotificationCompat;
import org.json.JSONObject;
import org.junit.Before;
import org.junit.Test;
import org.junit.runner.RunWith;
import org.mockito.ArgumentCaptor;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;
import org.robolectric.Robolectric;
import org.robolectric.RobolectricTestRunner;
import org.robolectric.annotation.Config;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import static org.junit.Assert.*;
import static org.mockito.Mockito.*;

package com.mokhoulani.ReactApp.modules;






@RunWith(RobolectricTestRunner.class)
@Config(sdk = Build.VERSION_CODES.P)
public class AppUsageServicePackageTest {

    @Mock
    UsageStatsManager usageStatsManager;

    @Mock
    NotificationManager notificationManager;

    @Before
    public void setUp() {
        MockitoAnnotations.initMocks(this);
    }

    private AppUsageService createServiceWithMocks(List<UsageStats> usageStatsList) {
        AppUsageService service = Mockito.spy(Robolectric.buildService(AppUsageService.class).create().get());

        // Mock getSystemService for USAGE_STATS_SERVICE and NOTIFICATION_SERVICE
        doReturn(usageStatsManager).when(service).getSystemService(Context.USAGE_STATS_SERVICE);
        doReturn(notificationManager).when(service).getSystemService(NotificationManager.class);

        // Mock queryUsageStats
        when(usageStatsManager.queryUsageStats(
                eq(UsageStatsManager.INTERVAL_DAILY),
                anyLong(),
                anyLong()
        )).thenReturn(usageStatsList);

        return service;
    }

    @Test
    public void testGetAppUsageData_returnsCorrectData() throws Exception {
        // Arrange
        UsageStats mockUsageStats = mock(UsageStats.class);
        when(mockUsageStats.getPackageName()).thenReturn("com.example.app");
        when(mockUsageStats.getTotalTimeInForeground()).thenReturn(5000L);

        List<UsageStats> usageStatsList = new ArrayList<>();
        usageStatsList.add(mockUsageStats);

        AppUsageService service = createServiceWithMocks(usageStatsList);

        // Act
        List<JSONObject> result = service.getAppUsageData();

        // Assert
        assertEquals(1, result.size());
        JSONObject obj = result.get(0);
        assertEquals("com.example.app", obj.getString("packageName"));
        assertEquals(5000L, obj.getLong("totalTimeInForeground"));
        assertTrue(obj.has("startTime"));
        assertTrue(obj.has("endTime"));
    }

    @Test
    public void testGetAppUsageData_filtersZeroForegroundTime() {
        // Arrange
        UsageStats mockUsageStats = mock(UsageStats.class);
        when(mockUsageStats.getPackageName()).thenReturn("com.example.app");
        when(mockUsageStats.getTotalTimeInForeground()).thenReturn(0L);

        List<UsageStats> usageStatsList = new ArrayList<>();
        usageStatsList.add(mockUsageStats);

        AppUsageService service = createServiceWithMocks(usageStatsList);

        // Act
        List<JSONObject> result = service.getAppUsageData();

        // Assert
        assertTrue(result.isEmpty());
    }

    @Test
    public void testOnStartCommand_startsForegroundAndSendsData() {
        // Arrange
        AppUsageService service = Mockito.spy(Robolectric.buildService(AppUsageService.class).create().get());
        doReturn(notificationManager).when(service).getSystemService(NotificationManager.class);
        doReturn(usageStatsManager).when(service).getSystemService(Context.USAGE_STATS_SERVICE);
        doReturn(new ArrayList<UsageStats>()).when(usageStatsManager).queryUsageStats(anyInt(), anyLong(), anyLong());

        // Mock ApiClient
        ApiClientMock apiClientMock = new ApiClientMock();
        ApiClient.setInstance(apiClientMock);

        // Act
        int result = service.onStartCommand(new Intent(), 0, 0);

        // Assert
        assertEquals(Service.START_NOT_STICKY, result);
        assertTrue(apiClientMock.sendUsageDataCalled);
    }

    // Helper mock for ApiClient
    public static class ApiClientMock extends ApiClient {
        public boolean sendUsageDataCalled = false;
        @Override
        public void sendUsageData(List<JSONObject> data) {
            sendUsageDataCalled = true;
        }
    }
}