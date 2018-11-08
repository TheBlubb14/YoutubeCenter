//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace YoutubeCenter.Helper
//{
//    class VisibilityHelper
//    {
//        <ListBox x:Name="FolderList" HorizontalAlignment="Left" Margin="10,10,0,10" Width="141.333" ItemsSource="{Binding BaseCollection}" SelectedItem="{Binding CurrentRecord}">
//            <ListBox.ItemTemplate>
//                <DataTemplate>
//                    <TextBlock Text = "{Binding Path=Name}" Tag="{Binding DataContext, RelativeSource={RelativeSource AncestorType=ListBox}}">
//                        <TextBlock.ContextMenu>
//                            <ContextMenu DataContext = "{Binding PlacementTarget.Tag, RelativeSource={RelativeSource Self}}"  Tag="{Binding Path=PlacementTarget}">
//                                <MenuItem Header = "Save" Command="{Binding SaeFolder}" CommandParameter="{Binding RelativeSource={RelativeSource TemplatedParent}, Path=Content}"/>
//                                <MenuItem Header = "Delete" Command="{Binding DveleteFolder}" CommandParameter="{Binding RelativeSource={RelativeSource TemplatedParent}, Path=Content}"/>
//                                <MenuItem Header = "Remove" Command="{Binding RemoveFolder}" CommandParameter="{Binding RelativeSource={RelativeSource TemplatedParent}, Path=Content}" Visibility="{Binding ReferenceMode}"/>
//                                <MenuItem Visible = "{Binding Visible}" />                
//            </ContextMenu>
//                        </TextBlock.ContextMenu>
//                    </TextBlock>
//                </DataTemplate>
//            </ListBox.ItemTemplate>
//            <ListBox.ContextMenu>
//                <ContextMenu>
//                    <MenuItem Header = "New" Command="{Binding NewFolder}"/>
//                    <MenuItem Header = "Add" Command="{Binding AddFolder}" Visibility="{Binding ReferenceMode}"/>
//                </ContextMenu>
//            </ListBox.ContextMenu>
//        </ListBox>
//    }
//}
