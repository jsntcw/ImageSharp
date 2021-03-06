﻿
namespace ImageSharp.Tests.Drawing
{
    using System;
    using System.IO;
    using ImageSharp;
    using ImageSharp.Drawing.Brushes;
    using Processing;
    using System.Collections.Generic;
    using Xunit;
    using ImageSharp.Drawing;
    using System.Numerics;
    using SixLabors.Shapes;
    using ImageSharp.Drawing.Processors;
    using ImageSharp.Drawing.Pens;
    using Moq;
    using System.Collections.Immutable;

    public class FillRegionProcessorTests
    {
        [Theory]
        [InlineData(true, 1, 4)]
        [InlineData(true, 2, 4)]
        [InlineData(true, 5, 5)]
        [InlineData(true, 8, 8)]
        [InlineData(false, 8, 4)] 
        [InlineData(false, 16, 4)] // we always do 4 sub=pixels when antialising is off.
        public void MinimumAntialiasSubpixelDepth(bool antialias, int antialiasSubpixelDepth, int expectedAntialiasSubpixelDepth)
        {
            var bounds = new ImageSharp.Rectangle(0, 0, 1, 1);

            Mock<IBrush<Color>> brush = new Mock<IBrush<Color>>();
            Mock<Region> region = new Mock<Region>();
            region.Setup(x => x.Bounds).Returns(bounds);

            GraphicsOptions options = new GraphicsOptions(antialias) {
                AntialiasSubpixelDepth = 1
            };
            FillRegionProcessor<Color> processor = new FillRegionProcessor<Color>(brush.Object, region.Object, options);
            Image img = new Image(1, 1);
            processor.Apply(img, bounds);

            region.Verify(x => x.Scan(It.IsAny<float>(), It.IsAny<float[]>(), It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(4));
        }
    }
}
