import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import * as z from "zod";
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Checkbox } from "@/components/ui/checkbox";
import { Loader2 } from "lucide-react";
import { useEffect } from "react";
import { useCreateCase, useUpdateCase } from "@/lib/abp/hooks/useCases";
import { toast } from "sonner";

const formSchema = z.object({

  caseNumber: z.any(),

  title: z.any(),

});

type FormValues = z.infer<typeof formSchema>;

interface CaseFormProps {
  isOpen: boolean;
  onClose: () => void;
  initialValues?: any;
}

export function CaseForm({
  isOpen,
  onClose,
  initialValues,
}: CaseFormProps) {
  const isEditing = !!initialValues;

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
    setValue,
    watch,
  } = useForm<FormValues>({
    resolver: zodResolver(formSchema),
    defaultValues: initialValues || {},
  });

  useEffect(() => {
    if (initialValues) {
      reset(initialValues);
    } else {
      reset({});
    }
  }, [initialValues, reset]);

  const createMutation = useCreateCase();
  const updateMutation = useUpdateCase();

  const onSubmit = async (data: FormValues) => {
    try {
      console.log("Submitting Case:", data);
      if (isEditing) {
        await updateMutation.mutateAsync({ id: initialValues.id, data });
        toast.success("Case updated successfully");
      } else {
        await createMutation.mutateAsync(data);
        toast.success("Case created successfully");
      }
      onClose();
    } catch (error: any) {
      console.error("Failed to save case:", error);
      toast.error(error.message || "Failed to save case");
    }
  };

  return (
    <Dialog open={isOpen} onOpenChange={onClose}>
      <DialogContent className="sm:max-w-[500px]">
        <DialogHeader>
          <DialogTitle>{isEditing ? "Edit Case" : "Create Case"}</DialogTitle>
          <DialogDescription>
            {isEditing ? "Update the details of the case." : "Fill in the details to create a new case."}
          </DialogDescription>
        </DialogHeader>
        <form onSubmit={handleSubmit(onSubmit)} className="space-y-4 py-4">

          <div className="space-y-2">
            <Label htmlFor="caseNumber">CaseNumber</Label>

            <Input id="caseNumber" {...register("caseNumber")} />

          </div>

          <div className="space-y-2">
            <Label htmlFor="title">Title</Label>

            <Input id="title" {...register("title")} />

          </div>


          <DialogFooter>
            <Button type="button" variant="outline" onClick={onClose} disabled={isSubmitting}>
              Cancel
            </Button>
            <Button type="submit" disabled={isSubmitting}>
              {isSubmitting && <Loader2 className="mr-2 h-4 w-4 animate-spin" />}
              {isEditing ? "Save Changes" : "Create"}
            </Button>
          </DialogFooter>
        </form>
      </DialogContent>
    </Dialog>
  );
}
