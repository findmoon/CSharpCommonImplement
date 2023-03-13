// 时间记录类
class TimeRecord {
  final Duration record; // 当前时刻
  final Duration addition; // 与上一时刻差值

  const TimeRecord({
    required this.record,
    required this.addition,
  });
}