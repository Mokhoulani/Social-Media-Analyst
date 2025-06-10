package com.ReactApp;

import android.content.Intent;
import android.os.Build;
import android.util.Log;

import com.facebook.react.bridge.ReactApplicationContext;
import com.facebook.react.bridge.ReactContextBaseJavaModule;
import com.facebook.react.bridge.ReactMethod;
import com.mokhoulani.ReactApp.services.AppUsageService;

public class StartAppUsageServiceModule extends ReactContextBaseJavaModule {

    StartAppUsageServiceModule(ReactApplicationContext context) {
        super(context);
    }

    @Override
    public String getName() {
        return "StartAppUsageService";
    }

    @ReactMethod
    public void start() {
        Log.d("StartAppUsageService", "Starting service from JS");

        Intent intent = new Intent(getReactApplicationContext(), AppUsageService.class);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            getReactApplicationContext().startForegroundService(intent);
        } else {
            getReactApplicationContext().startService(intent);
        }
    }
}