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
import { Checkbox } from "@/components/ui/checkbox";
import { Textarea } from "@/components/ui/textarea";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import { ArrowLeft, Save, Loader2, Plus, Trash2 } from "lucide-react";
import { toast } from "sonner";
import { 
  useProposal, 
  useCreateProposal, 
  useUpdateProposal 
} from "@/lib/abp/hooks/useProposals";


import { useClients } from "@/lib/abp/hooks/useClients";


const formSchema = z.object({
  
  number: z.any(),
  
  date: z.any(),
  
  validate: z.any(),
  
  obs: z.any(),
  
  clientId: z.any(),
  
});

type FormValues = z.infer<typeof formSchema>;



export default function ProposalFormPage() {
  const [, setLocation] = useLocation();
  const { id } = useParams<{ id: string }>();
  const isEditing = !!id;

  const { data: existing, isLoading: loadingExisting } = useProposal(id || "");
  const createMutation = useCreateProposal();
  const updateMutation = useUpdateProposal();

  
  
  const { data: clients } = useClients({ maxResultCount: 1000 });
  

  

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: {},
  });

  useEffect(() => {
    if (existing) {
      reset(existing);
      
    }
  }, [existing, reset]);

  

  const onSubmit = async (data: FormValues) => {
    try {
      const payload = {
        ...data,
        
      };

      if (isEditing) {
        await updateMutation.mutateAsync({ id: id!, data: payload });
        toast.success("Proposal updated successfully");
      } else {
        await createMutation.mutateAsync(payload);
        toast.success("Proposal created successfully");
      }
      setLocation("/admin/proposal");
    } catch (error: any) {
      toast.error(error.message || "Failed to save proposal");
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
        {/* Header */}
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

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
            {/* Main Content - 2 columns */}
            <div className="lg:col-span-2 space-y-6">
              {/* Basic Information Card */}
              <Card>
                <CardHeader>
                  <CardTitle>Basic Information</CardTitle>
                </CardHeader>
                <CardContent>
                  <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="number">
                        Number
                      </Label>
                      
                      <Input 
                        id="number" 
                        placeholder=""
                        {...register("number")} 
                      />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="date">
                        Date
                      </Label>
                      
                      <Input id="date" type="date" {...register("date")} />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="validate">
                        Validate
                      </Label>
                      
                      <Input id="validate" type="date" {...register("validate")} />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="obs">
                        Obs
                      </Label>
                      
                      <Textarea 
                        id="obs" 
                        rows={4}
                        placeholder=""
                        {...register("obs")} 
                      />
                      
                    </div>
                    
                    
                    <div className="space-y-2">
                      <Label htmlFor="clientId">
                        Client
                      </Label>
                      
                      <Select
                        value={watch("clientId")?.toString()}
                        onValueChange={(val) => setValue("clientId", val)}
                      >
                        <SelectTrigger>
                          <SelectValue placeholder="Select Name" />
                        </SelectTrigger>
                        <SelectContent>
                          { (clients as any)?.items?.map((item: any) => (
                            <SelectItem key={item.id} value={item.id}>
                              {item.Name}
                            </SelectItem>
                          ))}
                        </SelectContent>
                      </Select>
                      
                    </div>
                    
                  </div>
                </CardContent>
              </Card>

              

              
            </div>

            {/* Sidebar - 1 column */}
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
      </div>
    </Shell>
  );
}
