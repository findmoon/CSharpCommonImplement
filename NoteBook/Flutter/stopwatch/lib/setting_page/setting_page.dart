import 'package:flutter/material.dart';

import 'package:flex_color_picker/flex_color_picker.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

import '../app_state_bloc/app_state_bloc.dart';
import 'language_select_dialog.dart';

class SettingPage extends StatelessWidget {
  const SettingPage({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    String setting = AppLocalizations.of(context)!.setting;

    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        leading: const BackButton(color: Colors.black), // 标题栏最左侧显示。BackButton-返回组件
        title: Text(
          setting,
          style: TextStyle(color: Colors.black, fontSize: 16),
        ),
      ),
      body: Column(
        children: [
          buildColorItem(context),
          buildLocalItem(context),
        ],
      ),
    );
  }

  // ColorItem
  Widget buildColorItem(BuildContext context){
    String colorThemeTitle = AppLocalizations.of(context)!.colorThemeTitle;
    String colorThemeSubTitle = AppLocalizations.of(context)!.colorThemeSubTitle;

    return ListTile(
      onTap: () => _selectColor(context),
      title: Text(colorThemeTitle), // const Text('选取主题色'),
      subtitle: Text(colorThemeSubTitle), // const Text('秒表界面的高亮颜色为主题色'),
      trailing: Container(
        width: 26,
        height: 26,
        decoration: BoxDecoration(
          borderRadius: BorderRadius.circular(8),
          color: Theme.of(context).primaryColor,
        ),
      ),
    );
  }

  void _selectColor(BuildContext context) async {
    Color initColor = Theme.of(context).primaryColor;
    String colorDialogTitle = AppLocalizations.of(context)!.colorDialogTitle;

    // 显示颜色选择
    final Color newColor = await showColorPickerDialog(
      context,
      initColor,
      title: Text(colorDialogTitle, style: Theme.of(context).textTheme.titleLarge),
      // width: 40,
      // height: 40,
      spacing: 0,
      runSpacing: 0,
      borderRadius: 0,
      wheelDiameter: 165,
      enableOpacity: true,
      showColorCode: true,
      colorCodeHasColor: true,
      pickersEnabled: <ColorPickerType, bool>{
        ColorPickerType.wheel: true,
      },
      copyPasteBehavior: const ColorPickerCopyPasteBehavior(
        copyButton: false,
        pasteButton: false,
        longPressMenu: true,
      ),
      actionButtons: const ColorPickerActionButtons(
        okButton: true,
        closeButton: true,
        dialogActionButtons: false,
      ),
      constraints: const BoxConstraints(minHeight: 480, minWidth: 320, maxWidth: 320),
    );

    // 设置新颜色
    // 通过上下文获取 AppStateBloc 对象，执行 switchThemeColor 方法更新数据状态
    BlocProvider.of<AppStateBloc>(context).switchThemeColor(newColor);
  }

  // LocalItem
  Widget buildLocalItem(BuildContext context){
    String local = BlocProvider.of<AppStateBloc>(context).state.locale.languageCode;

    String localTitle = AppLocalizations.of(context)!.localTitle;
    String localSubTitle = AppLocalizations.of(context)!.localSubTitle;
    return ListTile(
      onTap: () => showLanguageSelectDialog(context),
      title:  Text(localTitle),
      subtitle: Text(localSubTitle),
      trailing: Container(
          width: 24,
          height: 24,
          alignment: Alignment.center,
          decoration: BoxDecoration(
            // color: Colors.black,
              shape: BoxShape.circle,
              border: Border.all()
          ),
          child: Text(local,
                    style: TextStyle(height: 1,
                              color: Theme.of(context).primaryColor
                    )
          )
      ),
    );
  }

}