using Microsoft.Extensions.Logging;

namespace Sysware.Core.Services;

/// <summary>
/// 示例服务，演示日志的使用
/// </summary>
public class ExampleService
{
    private readonly ILogger<ExampleService> _logger;

    public ExampleService(ILogger<ExampleService> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// 执行示例操作
    /// </summary>
    /// <param name="operationName">操作名称</param>
    /// <returns>操作结果</returns>
    public async Task<bool> ExecuteOperationAsync(string operationName)
    {
        _logger.LogInformation("开始执行操作: {OperationName}", operationName);
        
        try
        {
            // 模拟一些工作
            await Task.Delay(1000);
            
            // 模拟随机成功/失败
            var random = new Random();
            var success = random.Next(0, 10) > 2; // 80% 成功率
            
            if (success)
            {
                _logger.LogInformation("操作 {OperationName} 执行成功", operationName);
                return true;
            }
            else
            {
                _logger.LogWarning("操作 {OperationName} 执行失败", operationName);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "执行操作 {OperationName} 时发生异常", operationName);
            return false;
        }
    }

    /// <summary>
    /// 处理数据
    /// </summary>
    /// <param name="data">要处理的数据</param>
    public void ProcessData(object data)
    {
        _logger.LogDebug("开始处理数据: {Data}", data);
        
        if (data == null)
        {
            _logger.LogWarning("接收到空数据，跳过处理");
            return;
        }

        try
        {
            // 模拟数据处理
            var dataType = data.GetType().Name;
            _logger.LogTrace("数据类型: {DataType}", dataType);
            
            // 这里可以添加实际的数据处理逻辑
            _logger.LogInformation("数据处理完成，类型: {DataType}", dataType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "处理数据时发生错误: {Data}", data);
            throw;
        }
    }
}
