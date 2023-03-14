import 'dart:ui';

import 'package:flutter_bloc/flutter_bloc.dart';

import 'app_state.dart';

class AppStateBloc extends Cubit<AppState> {
  AppStateBloc({required AppState appConfig}) : super(appConfig);

  void switchThemeColor(Color color) {
    if (color != state.themeColor) {
      emit(state.copyWith(color: color));
    }
  }

  void switchLanguage(Locale locale) {
    if (locale != state.locale) {
      emit(state.copyWith(locale: locale));
    }
  }

}
