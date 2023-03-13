import 'package:flutter/material.dart';
import '../models/time_record.dart';

class RecordPanel extends StatelessWidget {
  final List<TimeRecord> records;

  const RecordPanel({super.key,required this.records});

  @override
  Widget build(BuildContext context) {
    // 根据数据 构建列表组件
    return Padding(
      padding: const EdgeInsets.only( top: 20),
      child: DefaultTextStyle(
        style: const TextStyle(fontFamily: 'IBMPlexMono', color: Colors.black),
        child: ListView.builder(
          itemBuilder: _buildItemByIndex,
          itemCount: records.length,
        ),
      ),
    );
  }


  final EdgeInsets itemPadding = const EdgeInsets.symmetric(horizontal: 20, vertical: 4);
  Widget _buildItemByIndex(BuildContext context, int index) {
    // 反转索引，实现倒序排列 【很巧妙的实现】
    int reverseIndex = (records.length - 1) - index; // 反转索引
    Color themeColor = Theme.of(context).primaryColor;
    Color? indexColor = (reverseIndex == records.length - 1) ? themeColor : null;
    return Row(
      children: [
        Padding(
          padding: itemPadding,
          child: Text(
              // index.toString().padLeft(2, '0'),
              reverseIndex.toString().padLeft(2, '0'),
              style: TextStyle(color: indexColor)
          ),
        ),
        // Text(durationToString(records[index].record)),
        Text(durationToString(records[reverseIndex].record)),
        const Spacer(), // 占据剩余空间
        Padding(
          padding: itemPadding,
          // child: Text("+" + durationToString(records[index].addition)),
          child: Text("+" + durationToString(records[reverseIndex].addition)),
        ),
      ],
    );
  }

  //
  String durationToString(Duration duration) {
    int minus = duration.inMinutes % 60;
    int second = duration.inSeconds % 60;
    int milliseconds = duration.inMilliseconds % 1000;
    String commonStr = '${minus.toString().padLeft(2, "0")}:${second.toString().padLeft(2, "0")}';
    String highlightStr = ".${(milliseconds ~/ 10).toString().padLeft(2, "0")}";
    return commonStr + highlightStr;
  }
}