import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import { useLocation, useParams } from "wouter";
import { Shell } from "@/components/layout/shell";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Textarea } from "../../../components/ui/textarea";
import { ArrowLeft, Save, Loader2, Plus, Trash2, Pencil, MoreHorizontal } from "lucide-react";
import { toast } from "sonner";
import { Tabs, TabsContent, TabsList, TabsTrigger } from "@/components/ui/tabs";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import {
  DropdownMenu,
  DropdownMenuContent,
  DropdownMenuItem,
  DropdownMenuLabel,
  DropdownMenuSeparator,
  DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import {
  useProposal,
  useCreateProposal,
  useUpdateProposal
} from "@/lib/abp/hooks/useProposals";
import {
  usePropostalItems,
  useDeletePropostalItem
} from "@/lib/abp/hooks/usePropostalItems";

const formSchema = z.object({
  number: z.string().optional(),
  date: z.string().optional(),
  validate: z.string().optional(),
  obs: z.string().optional(),
  clientId: z.string().optional(),
});

type FormValues = z.infer<typeof formSchema>;

export default function ProposalFormPage() {
  const [, setLocation] = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = !!id;

  const { data: existing, isLoading: loadingExisting } = useProposal(id || "");
  const createMutation = useCreateProposal();
  const updateMutation = useUpdateProposal();

  const { data: itemsData, isLoading: loadingItems } = usePropostalItems({ proposalId: id });
  const deleteItemMutation = useDeletePropostalItem();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {},
  });

  useEffect(() => {
    if (existing) {
      // Format dates for input[type="date"]
      const formatted = {
        ...existing,
        date: existing.date ? new Date(existing.date).toISOString().split('T')[0] : "",
        validate: existing.validate ? new Date(existing.validate).toISOString().split('T')[0] : "",
      };
      reset(formatted);
    }
  }, [existing, reset]);

  const onSubmit = async (data: FormValues) => {
    try {
      if (isEditing) {
        await updateMutation.mutateAsync({ id: id!, data });
        toast.success("Proposal updated successfully");
      } else {
        const result = await createMutation.mutateAsync(data);
        toast.success("Proposal created successfully");
        setLocation(`/admin/proposal/${result.id}/edit`);
      }
    } catch (error: any) {
      toast.error(error.message || "Failed to save proposal");
    }
  };

  const handleDeleteItem = async (itemId: string) => {
    if (confirm("Are you sure you want to delete this item?")) {
      try {
        await deleteItemMutation.mutateAsync(itemId);
        toast.success("Item deleted successfully");
      } catch (error: any) {
        toast.error("Failed to delete item");
      }
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  if (loadingExisting && isEditing) {
    return (
      <Shell>
        <div className="flex items-center justify-center h-64">
          <Loader2 className="h-8 w-8 animate-spin text-primary" />
        </div>
      </Shell>
    );
  }

  return (
    <Shell>
      <div className="space-y-6">
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-4">
            <Button
              variant="ghost"
              size="icon"
              onClick={() => setLocation("/admin/proposal")}
            >
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold tracking-tight">
                {isEditing ? "Edit Proposal" : "New Proposal"}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? "Update the details" : "Create a new proposal"}
              </p>
            </div>
          </div>
        </div>

        <Tabs defaultValue="details" className="w-full">
          <TabsList className="bg-muted/50 p-1">
            <TabsTrigger value="details">General Information</TabsTrigger>
            {isEditing && (
              <TabsTrigger value="items">Items ({itemsData?.totalCount || 0})</TabsTrigger>
            )}
          </TabsList>

          <TabsContent value="details" className="mt-6">
            <form onSubmit={handleSubmit(onSubmit)}>
              <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                <div className="lg:col-span-2 space-y-6">
                  <Card>
                    <CardHeader>
                      <CardTitle>Basic Information</CardTitle>
                    </CardHeader>
                    <CardContent>
                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <Label htmlFor="number">Number</Label>
                          <Input id="number" {...register("number")} />
                        </div>
                        <div className="space-y-2">
                          <Label htmlFor="date">Date</Label>
                          <Input id="date" type="date" {...register("date")} />
                        </div>
                        <div className="space-y-2">
                          <Label htmlFor="validate">Validity Date</Label>
                          <Input id="validate" type="date" {...register("validate")} />
                        </div>
                        <div className="space-y-2">
                          <Label htmlFor="clientId">Client ID</Label>
                          <Input id="clientId" {...register("clientId")} />
                        </div>
                        <div className="col-span-2 space-y-2">
                          <Label htmlFor="obs">Observations</Label>
                          <Textarea id="obs" rows={4} {...register("obs")} />
                        </div>
                      </div>
                    </CardContent>
                  </Card>
                </div>

                <div className="space-y-6">
                  <Card className="sticky top-6">
                    <CardHeader>
                      <CardTitle>Actions</CardTitle>
                    </CardHeader>
                    <CardContent className="space-y-3">
                      <Button type="submit" className="w-full" disabled={isLoading}>
                        {isLoading && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
                        <Save className="mr-2 h-4 w-4" />
                        {isEditing ? "Save Changes" : "Create Proposal"}
                      </Button>
                      <Button
                        type="button"
                        variant="outline"
                        className="w-full"
                        onClick={() => setLocation("/admin/proposal")}
                      >
                        Cancel
                      </Button>
                    </CardContent>
                  </Card>
                </div>
              </div>
            </form>
          </TabsContent>

          {isEditing && (
            <TabsContent value="items" className="mt-6">
              <Card>
                <CardHeader className="flex flex-row items-center justify-between">
                  <div>
                    <CardTitle>Proposal Items</CardTitle>
                    <p className="text-sm text-muted-foreground mt-1">Manage items for this proposal</p>
                  </div>
                  <Button
                    size="sm"
                    className="gap-2"
                    onClick={() => setLocation(`/admin/propostal-item/new?proposalId=${id}`)}
                  >
                    <Plus className="size-4" />
                    Add Item
                  </Button>
                </CardHeader>
                <CardContent className="p-0">
                  <Table>
                    <TableHeader>
                      <TableRow>
                        <TableHead>Description</TableHead>
                        <TableHead>Quantity</TableHead>
                        <TableHead>Unit Price</TableHead>
                        <TableHead>Total</TableHead>
                        <TableHead className="text-right">Actions</TableHead>
                      </TableRow>
                    </TableHeader>
                    <TableBody>
                      {itemsData?.items?.map((item: any) => (
                        <TableRow key={item.id}>
                          <TableCell>{item.desc}</TableCell>
                          <TableCell>{item.quant}</TableCell>
                          <TableCell>{item.unitPrice}</TableCell>
                          <TableCell>{item.total}</TableCell>
                          <TableCell className="text-right">
                            <DropdownMenu>
                              <DropdownMenuTrigger asChild>
                                <Button variant="ghost" size="icon">
                                  <MoreHorizontal className="h-4 w-4" />
                                </Button>
                              </DropdownMenuTrigger>
                              <DropdownMenuContent align="end">
                                <DropdownMenuItem onClick={() => setLocation(`/admin/propostal-item/${item.id}/edit`)}>
                                  <Pencil className="mr-2 h-4 w-4" />
                                  Edit
                                </DropdownMenuItem>
                                <DropdownMenuItem
                                  className="text-destructive"
                                  onClick={() => handleDeleteItem(item.id)}
                                >
                                  <Trash2 className="mr-2 h-4 w-4" />
                                  Delete
                                </DropdownMenuItem>
                              </DropdownMenuContent>
                            </DropdownMenu>
                          </TableCell>
                        </TableRow>
                      ))}
                      {(!itemsData?.items || itemsData.items.length === 0) && (
                        <TableRow>
                          <TableCell colSpan={5} className="h-24 text-center text-muted-foreground">
                            No items found. Click "Add Item" to begin.
                          </TableCell>
                        </TableRow>
                      )}
                    </TableBody>
                  </Table>
                </CardContent>
              </Card>
            </TabsContent>
          )}
        </Tabs>
      </div>
    </Shell>
  );
}
