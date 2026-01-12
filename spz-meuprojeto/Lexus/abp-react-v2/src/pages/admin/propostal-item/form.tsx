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
import { ArrowLeft, Save, Loader2 } from "lucide-react";
import { toast } from "sonner";
import {
  usePropostalItem,
  useCreatePropostalItem,
  useUpdatePropostalItem
} from "@/lib/abp/hooks/usePropostalItems";

const formSchema = z.object({
  desc: z.string().optional(),
  quant: z.any().optional(),
  unitPrice: z.any().optional(),
  total: z.any().optional(),
  proposalId: z.string().optional(),
});

type FormValues = z.infer<typeof formSchema>;

export default function PropostalItemFormPage() {
  const [, setLocation] = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = !!id;

  // Get proposalId from query string
  const searchParams = new URLSearchParams(window.location.search);
  const proposalIdParam = searchParams.get("proposalId");

  const { data: existing, isLoading: loadingExisting } = usePropostalItem(id || "");
  const createMutation = useCreatePropostalItem();
  const updateMutation = useUpdatePropostalItem();

  const {
    register,
    handleSubmit,
    formState: { errors },
    reset,
    watch,
    setValue,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      proposalId: proposalIdParam || "",
    },
  });

  // Calculate total automatically
  const quant = watch("quant");
  const unitPrice = watch("unitPrice");

  useEffect(() => {
    if (quant !== undefined && unitPrice !== undefined) {
      setValue("total", Number(quant) * Number(unitPrice));
    }
  }, [quant, unitPrice, setValue]);

  useEffect(() => {
    if (existing) {
      reset(existing);
    } else if (proposalIdParam) {
      setValue("proposalId", proposalIdParam);
    }
  }, [existing, reset, proposalIdParam, setValue]);

  const onSubmit = async (data: FormValues) => {
    try {
      if (isEditing) {
        await updateMutation.mutateAsync({ id: id!, data });
        toast.success("Item updated successfully");
      } else {
        await createMutation.mutateAsync(data);
        toast.success("Item created successfully");
      }

      // Redirect back to proposal if we have a proposalId
      const targetProposalId = data.proposalId || existing?.proposalId || proposalIdParam;
      if (targetProposalId) {
        setLocation(`/admin/proposal/${targetProposalId}/edit`);
      } else {
        setLocation("/admin/propostal-item");
      }
    } catch (error: any) {
      toast.error(error.message || "Failed to save item");
    }
  };

  const isLoading = createMutation.isPending || updateMutation.isPending;

  const handleBack = () => {
    const targetProposalId = watch("proposalId") || existing?.proposalId || proposalIdParam;
    if (targetProposalId) {
      setLocation(`/admin/proposal/${targetProposalId}/edit`);
    } else {
      setLocation("/admin/propostal-item");
    }
  };

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
            <Button variant="ghost" size="icon" onClick={handleBack}>
              <ArrowLeft className="h-5 w-5" />
            </Button>
            <div>
              <h1 className="text-2xl font-bold tracking-tight">
                {isEditing ? "Edit Item" : "New Item"}
              </h1>
              <p className="text-muted-foreground">
                {isEditing ? "Update item details" : "Add a new item to the proposal"}
              </p>
            </div>
          </div>
        </div>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            <div className="lg:col-span-2 space-y-6">
              <Card>
                <CardHeader>
                  <CardTitle>Item Details</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div className="col-span-2 space-y-2">
                      <Label htmlFor="desc">Description</Label>
                      <Input id="desc" {...register("desc")} />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="quant">Quantity</Label>
                      <Input id="quant" type="number" step="any" {...register("quant")} />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="unitPrice">Unit Price</Label>
                      <Input id="unitPrice" type="number" step="any" {...register("unitPrice")} />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="total">Total</Label>
                      <Input id="total" type="number" step="any" {...register("total")} readOnly className="bg-muted" />
                    </div>

                    <div className="space-y-2">
                      <Label htmlFor="proposalId">Proposal ID</Label>
                      <Input id="proposalId" {...register("proposalId")} readOnly className="bg-muted" />
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
                    {isEditing ? "Save Changes" : "Create Item"}
                  </Button>
                  <Button
                    type="button"
                    variant="outline"
                    className="w-full"
                    onClick={handleBack}
                  >
                    Cancel
                  </Button>
                </CardContent>
              </Card>
            </div>
          </div>
        </form>
      </div>
    </Shell>
  );
}
