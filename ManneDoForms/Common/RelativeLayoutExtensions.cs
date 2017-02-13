using Xamarin.Forms;

namespace ManneDoForms.Common
{
    public static class RelativeLayoutExtensions
    {
        public static void AddLeft(this RelativeLayout.IRelativeList<View> layout, View view, int width, int height, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return x;
                }),
                yConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    else return arg.Width + width;
                }),
                heightConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (height >= 0)
                        return height > 0 ? height : arg.Height - y * 2;
                    return arg.Height + height;
                }));
        }

        public static void AddLeft(this RelativeLayout.IRelativeList<View> layout, View view, int width, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (x > 0)
                        return x;
                    return arg.Width - width + x;
                }),
                yConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    return arg.Width + width;
                }));
        }

        public static void AddRight(this RelativeLayout.IRelativeList<View> layout, View view, int width, int height, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return arg.Width - width - x;
                }),
                yConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    else return arg.Width + width;
                }),
                heightConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (height >= 0)
                        return height > 0 ? height : arg.Height - y * 2;
                    return arg.Height + height;
                }));
        }

        public static void AddRight(this RelativeLayout.IRelativeList<View> layout, View view, int width, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return arg.Width - width - x;
                }),
                yConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    return arg.Width + width;
                }));
        }

        public static void AddLeftAfter(this RelativeLayout.IRelativeList<View> layout, View view, View prev, int width, int height, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (x > 0)
                        return x;
                    return arg.Width + x;
                }),
                yConstraint: Constraint.RelativeToView(prev, (parent, arg) =>
                {
                    return arg.Y + arg.Height + y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    return arg.Width + width;
                }),
                heightConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (height >= 0)
                        return height > 0 ? height : arg.Height - y * 2;
                    return arg.Height + height;
                }));
        }

        public static void AddLeftAfter(this RelativeLayout.IRelativeList<View> layout, View view, View prev, int width, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return x;
                }),
                yConstraint: Constraint.RelativeToView(prev, (parent, arg) =>
                {
                    return arg.Y + arg.Height + y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    return arg.Width + width;
                }));
        }

        public static void AddRightAfter(this RelativeLayout.IRelativeList<View> layout, View view, View prev, int width, int height, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return arg.Width - width - x;
                }),
                yConstraint: Constraint.RelativeToView(prev, (parent, arg) =>
                {
                    return arg.Y + arg.Height + y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    return arg.Width + width;
                }),
                heightConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (height >= 0)
                        return height > 0 ? height : arg.Height - y * 2;
                    return arg.Height + height;
                }));
        }

        public static void AddRightAfter(this RelativeLayout.IRelativeList<View> layout, View view, View prev, int width, int x, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return arg.Width - width - x;
                }),
                yConstraint: Constraint.RelativeToView(prev, (parent, arg) =>
                {
                    return arg.Y + arg.Height + y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width >= 0)
                        return width > 0 ? width : arg.Width - x * 2;
                    return arg.Width + width;
                }));
        }

        public static void AddCentered(this RelativeLayout.IRelativeList<View> layout, View view, int width, int height, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width > 0)
                        return (arg.Width - width) / 2;
                    return 0 - width / 2;
                }),
                yConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return width > 0 ? width : arg.Width + width;
                }),
                heightConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return height > 0 ? height : arg.Height;
                }));
        }

        public static void AddCentered(this RelativeLayout.IRelativeList<View> layout, View view, int width, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width > 0)
                        return (arg.Width - width) / 2;
                    return 0 - width / 2;
                }),
                yConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return width > 0 ? width : arg.Width + width;
                }));
        }

        public static void AddAfter(this RelativeLayout.IRelativeList<View> layout, View view, View prev, int width, int y)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width > 0)
                        return (arg.Width - width) / 2;
                    return 0 - width / 2;
                }),
                yConstraint: Constraint.RelativeToView(prev, (parent, arg) =>
                {
                    return arg.Y + arg.Height + y;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return width > 0 ? width : arg.Width + width;
                }));
        }

        public static void AddInOrigo(this RelativeLayout.IRelativeList<View> layout, View view, int width, int height)
        {
            layout.Add(view,
                xConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (width > 0)
                        return (arg.Width - width) / 2;
                    return 0 - width / 2;
                }),
                yConstraint: Constraint.RelativeToParent((arg) =>
                {
                    if (height > 0)
                        return (arg.Height - height) / 2;
                    return 0 - height / 2;
                }),
                widthConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return width > 0 ? width : arg.Width + width;
                }),
                heightConstraint: Constraint.RelativeToParent((arg) =>
                {
                    return height > 0 ? height : arg.Height;
                }));
        }
    }
}