using Moq;
using System.Diagnostics;
using System.Reflection;
using Shouldly.Configuration;
using System.Runtime.CompilerServices;

namespace Shouldly.Tests.Configuration;

public class FirstNonShouldlyMethodFinderTests
{
    [Fact]
    public void FirstNonShouldMethodFinder_NoStackFrameAtSpecifiedOffset_InvalidOperationExceptionTest()
    {
        var mockStackTrace = new Mock<StackTrace>();
        var mockFrame = new Mock<StackFrame>();
        var mockMethodBase = new Mock<MethodBase>();

        mockMethodBase.Setup(m => m.Name).Returns("YourTestMethod");
        mockFrame.Setup(f => f.GetMethod()).Returns(mockMethodBase.Object);
        mockStackTrace.Setup(st => st.GetFrames()).Returns(new StackFrame[] { mockFrame.Object });

        var sut = new FirstNonShouldlyMethodFinder();
        var exception = Should.Throw<InvalidOperationException>(() =>
        {
            sut.GetTestMethodInfo(mockStackTrace.Object);
        });
        exception.Message.ShouldBe("There is no stack frame at the specified offset from the first non-Shouldly stack frame.");

    }

    [Fact]
    public void GetTestMethodInfo_NoNonShouldlyMethodInStackTrace_InvalidOperationExceptionTest()
    {
        var mockStackTrace = new Mock<StackTrace>();
        var mockFrame = new Mock<StackFrame>();

        mockStackTrace.Setup(s => s.GetFrames()).Returns(new StackFrame[] { mockFrame.Object });
        mockFrame.Setup(f => f.GetMethod()).Returns(It.IsAny<MethodBase>());

        var sut = new FirstNonShouldlyMethodFinder();
        var exception = Should.Throw<InvalidOperationException>(() =>
        {
            sut.GetTestMethodInfo(mockStackTrace.Object);
        });
        exception.Message.ShouldBe("Cannot find a non-Shouldly method in the stack trace.");
    }

    [Fact]
    public void IsCompilerGenerated_ShouldReturnTrue()
    {
        MethodInfo? methodInfo = typeof(FirstNonShouldlyMethodFinder).GetMethod("IsCompilerGenerated", BindingFlags.NonPublic | BindingFlags.Static);
        var method = new Mock<MethodBase>();
        method.Setup(m => m.IsDefined(typeof(CompilerGeneratedAttribute), true)).Returns(true);
        methodInfo.ShouldNotBeNull();

        bool? result = (bool?)methodInfo!.Invoke(null, new object[] { method.Object });

        result.ShouldNotBeNull();
        result.ShouldBe(true);
    }

    [Fact]
    public void IsCompilerGenerated_ShouldReturnFalse()
    {
        MethodInfo? methodInfo = typeof(FirstNonShouldlyMethodFinder).GetMethod("IsCompilerGenerated", BindingFlags.NonPublic | BindingFlags.Static);
        var method = new Mock<MethodBase>();
        method.Setup(m => m.IsDefined(typeof(CompilerGeneratedAttribute), true)).Returns(false);
        method.Setup(m => m.Name).Returns("TestMethodName");
        methodInfo.ShouldNotBeNull();

        bool? result = (bool?)methodInfo!.Invoke(null, new object[] { method.Object });

        result.ShouldNotBeNull();
        result.ShouldBe(false);
    }

}
