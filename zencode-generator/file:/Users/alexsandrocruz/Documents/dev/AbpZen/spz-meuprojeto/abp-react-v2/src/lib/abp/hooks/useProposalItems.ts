import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetProposalItemsInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  proposalId?: string;
  }

export function useProposalItems(input: GetProposalItemsInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["proposalItems", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal-item", {
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

export function useAllProposalItems() {
  return useQuery({
    queryKey: ["proposalItems", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/proposal-item", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useProposalItem(id: string) {
  return useQuery({
    queryKey: ["proposalItem", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/proposal-item/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateProposalItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/proposal-item", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposalItems"] });
    },
  });
}

export function useUpdateProposalItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/proposal-item/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["proposalItems"] });
      queryClient.invalidateQueries({ queryKey: ["proposalItem", data.id] });
    },
  });
}

export function useDeleteProposalItem() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/proposal-item/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["proposalItems"] });
    },
  });
}


