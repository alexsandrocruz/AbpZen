import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLeadsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  leadWorkflowId?: string;
  leadWorkflowStageId?: string;
  }

export function useLeads(input: GetLeadsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["leads", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllLeads() {
  return useQuery({
    queryKey: ["leads", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lead", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useLead(id: string) {
  return useQuery({
    queryKey: ["lead", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lead/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateLead() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/lead", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leads"] });
    },
  });
}

export function useUpdateLead() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/lead/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["leads"] });
      queryClient.invalidateQueries({ queryKey: ["lead", data.id] });
    },
  });
}

export function useDeleteLead() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/lead/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leads"] });
    },
  });
}





/**
 * Toggle hook for many-to-many relationship: LeadWorkflow <-> LeadWorkflowStage
 * Given a LeadWorkflow, toggle a LeadWorkflowStage
 */
export function useToggleLeadWorkflowStage(leadWorkflowId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateLead();
  const deleteMutation = useDeleteLead();
  const { data: existing } = useLeads({ leadWorkflowId: leadWorkflowId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ leadWorkflowStageId, isChecked }: { leadWorkflowStageId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.leadWorkflowStageId === leadWorkflowStageId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          leadWorkflowId: leadWorkflowId,
          leadWorkflowStageId: leadWorkflowStageId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leads"] });
    },
  });
}

/**
 * Toggle hook for many-to-many relationship: LeadWorkflow <-> LeadWorkflowStage
 * Given a LeadWorkflowStage, toggle a LeadWorkflow
 */
export function useToggleLeadWorkflow(leadWorkflowStageId: string) {
  const queryClient = useQueryClient();
  const createMutation = useCreateLead();
  const deleteMutation = useDeleteLead();
  const { data: existing } = useLeads({ leadWorkflowStageId: leadWorkflowStageId, maxResultCount: 1000 });

  return useMutation({
    mutationFn: async ({ leadWorkflowId, isChecked }: { leadWorkflowId: string; isChecked: boolean }) => {
      if (isChecked) {
        // Remove relationship
        const record = existing?.items?.find((i: any) => i.leadWorkflowId === leadWorkflowId);
        if (record) {
          await deleteMutation.mutateAsync(record.id);
        }
      } else {
        // Add relationship
        await createMutation.mutateAsync({
          leadWorkflowId: leadWorkflowId,
          leadWorkflowStageId: leadWorkflowStageId,
        });
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["leads"] });
    },
  });
}

